using System;
using System.Collections.Generic;

namespace OmniKassa
{
    /// <summary>
    /// TokenProvider storing its values in memory
    /// </summary>
    public sealed class InMemoryTokenProvider : TokenProvider
    {
        private Dictionary<FieldName, String> mMap = new Dictionary<FieldName, String>();

        /// <summary>
        /// Initializes a new instance of the InMemoryTokenProvider class.
        /// </summary>
        /// <param name="refreshToken"></param>
        public InMemoryTokenProvider(String refreshToken)
        {
            SetValue(FieldName.REFRESH_TOKEN, refreshToken);
        }

        /// <summary>
        /// Gets the value of the stored key
        /// </summary>
        /// <param name="key">FieldName of the value which should be retrieved</param>
        /// <returns>String value of the stored key</returns>
        protected override String GetValue(FieldName key)
        {
            mMap.TryGetValue(key, out string value);
            return value;
        }

        /// <summary>
        /// Sets the combination of a key and a value
        /// </summary>
        /// <param name="key">FieldName of the value which should be retrieved</param>
        /// <param name="value">String value of max 1024 characters to be stored</param>
        protected override void SetValue(FieldName key, String value)
        {
            if (mMap.ContainsKey(key))
            {
                mMap[key] = value;
            }
            else
            {
                mMap.Add(key, value);
            }
        }
    }
}
