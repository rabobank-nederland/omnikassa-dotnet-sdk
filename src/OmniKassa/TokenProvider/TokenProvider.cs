using System;
using OmniKassa.Model;
using OmniKassa.Utils;

namespace OmniKassa
{
    /// <summary>
    /// The token provider is needed to store and retrieve the values of a token in key value pairs.
    /// The abstract class requires that the <see cref="GetValue(TokenProvider.FieldName)"/> and the <see cref="SetValue(TokenProvider.FieldName, string)"/> functions are implemented.
    /// The implementation is entirely up to the developer. For example the values can be persisted in a database and cached.
    /// The only thing to note, however, is that the refresh token is provided by *your* implementation and should have been supplied by the Rabobank together with this SDK release.
    /// </summary>
    public abstract class TokenProvider
    {
        /// <summary>
        /// Types of fields that can be stored
        /// </summary>
        public enum FieldName
        {
            /// <summary>
            /// Refresh token for API access
            /// </summary>
            REFRESH_TOKEN,

            /// <summary>
            /// Access token for API access
            /// </summary>
            ACCESS_TOKEN,

            /// <summary>
            /// Token validity date
            /// </summary>
            ACCESS_TOKEN_VALID_UNTIL,

            /// <summary>
            /// Token duration validity
            /// </summary>
            ACCESS_TOKEN_DURATION
        }

        private AccessToken accessToken;

        /// <summary>
        /// Gets the access token
        /// </summary>
        /// <returns>Access token</returns>
        public String GetAccessToken()
        {
            if (accessToken == null && HasAccessToken())
            {
                accessToken = ReCreateAccessToken();
            }

            if (accessToken != null && accessToken.IsNotExpired())
            {
                return accessToken.Token;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Whether or not the access token is valid
        /// </summary>
        /// <returns>true if the token is not valid, otherwise false</returns>
        public Boolean HasNoValidAccessToken()
        {
            return GetAccessToken() == null;
        }

        /// <summary>
        /// Gets the refresh token
        /// </summary>
        /// <returns>Refresh token</returns>
        public String GetRefreshToken()
        {
            return GetValue(FieldName.REFRESH_TOKEN);
        }

        /// <summary>
        /// Sets the access token
        /// </summary>
        /// <param name="accessToken">Access token</param>
        public void SetAccessToken(AccessToken accessToken)
        {
            this.accessToken = accessToken;
            SetValue(FieldName.ACCESS_TOKEN, accessToken.Token);
            SetValue(FieldName.ACCESS_TOKEN_VALID_UNTIL, DateTimeUtils.DateToString((DateTime)accessToken.ValidUntil));
            SetValue(FieldName.ACCESS_TOKEN_DURATION, Convert.ToString(accessToken.DurationInMillis));
        }

        private AccessToken ReCreateAccessToken()
        {
            String token = GetValue(FieldName.ACCESS_TOKEN);
            String validUntil = GetValue(FieldName.ACCESS_TOKEN_VALID_UNTIL);
            String durationInMillis = GetValue(FieldName.ACCESS_TOKEN_DURATION);

            return new AccessToken(token, DateTimeUtils.StringToDate(validUntil), Int32.Parse(durationInMillis));
        }

        private Boolean HasAccessToken()
        {
            String token = GetValue(FieldName.ACCESS_TOKEN);
            String validUntil = GetValue(FieldName.ACCESS_TOKEN_VALID_UNTIL);
            DateTime validUntilCalendar = DateTimeUtils.StringToDate(validUntil);
            String durationInMillis = GetValue(FieldName.ACCESS_TOKEN_DURATION);

            return !String.IsNullOrEmpty(token) && 
                   !String.IsNullOrEmpty(validUntil) && 
                   !String.IsNullOrEmpty(durationInMillis) && 
                          validUntilCalendar != DateTime.MinValue;
        }

        /// <summary>
        /// This function should retrieve the appropriate value for the key
        /// </summary>
        /// <param name="key">FieldName of the value which should be retrieved</param>
        /// <returns>String value of the stored key</returns>
        protected abstract String GetValue(FieldName key);

        /// <summary>
        /// This function should store the combination of a key and a value
        /// </summary>
        /// <param name="key">FieldName of the value which should be retrieved</param>
        /// <param name="value">String value of max 1024 characters to be stored</param>
        protected abstract void SetValue(FieldName key, String value);
    }
}
