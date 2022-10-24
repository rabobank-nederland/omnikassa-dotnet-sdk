using System;
using System.Collections.Generic;
using OmniKassa.Model;
using OmniKassa.Utils;
using OmniKassa.Tests.Model;
using Xunit;
using static OmniKassa.TokenProvider;

namespace OmniKassa.Tests.Connector
{
    public class TokenProviderTest
    {
        private static readonly String PAST_VALID_UNTIL = "2016-09-22T10:10:04.848+0200";
        private static readonly String FUTURE_VALID_UNTIL = InitializeFutureDate();

        private static String InitializeFutureDate()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddMonths(1);
            return DateTimeUtils.DateToString(dateTime);
        }

        private Dictionary<FieldName, String> mMap = new Dictionary<FieldName, string>();

        private TokenProvider mTokenProvider;

        public TokenProviderTest()
        {
            mTokenProvider = new TokenProviderSpy(mMap);
        }

        private String Get(FieldName field)
        {
            mMap.TryGetValue(field, out string value);
            return value;
        }

        private void Set(FieldName field, String value)
        {
            if(mMap.ContainsKey(field))
            {
                mMap[field] = value;
            }
            else
            {
                mMap.Add(field, value);
            }
        }

        [Fact]
        public void GetRefreshToken()
        {
            Set(FieldName.REFRESH_TOKEN, "refreshToken");

            Assert.Equal("refreshToken", mTokenProvider.GetRefreshToken());
        }

        [Fact]
        public void SetAccessToken()
        {
            AccessToken accessToken = new AccessTokenBuilder().WithValidUntilTomorrow().Build();

            String validUntilString = DateTimeUtils.DateToString((DateTime)accessToken.ValidUntil);

            mTokenProvider.SetAccessToken(accessToken);

            Set(FieldName.ACCESS_TOKEN, "token");
            Set(FieldName.ACCESS_TOKEN_VALID_UNTIL, validUntilString);
            Set(FieldName.ACCESS_TOKEN_DURATION, "28800000");

            Assert.Equal("token", Get(FieldName.ACCESS_TOKEN));
            Assert.Equal(validUntilString, Get(FieldName.ACCESS_TOKEN_VALID_UNTIL));
            Assert.Equal("28800000", Get(FieldName.ACCESS_TOKEN_DURATION));
        }

        [Fact]
        public void SetAndGetAccessToken()
        {
            AccessToken accessToken = new AccessTokenBuilder().WithValidUntilTomorrow().Build();

            mTokenProvider.SetAccessToken(accessToken);

            // token is valid and cache is used
            Assert.Equal("token", mTokenProvider.GetAccessToken());
            Assert.False(mTokenProvider.HasNoValidAccessToken());
        }

        [Fact]
        public void SetAndGetAccessToken_Expired()
        {
            AccessToken accessToken = new AccessTokenBuilder().Build();

            mTokenProvider.SetAccessToken(accessToken);

            // token is not valid, cache is not used
            Assert.Null(mTokenProvider.GetAccessToken());
            Assert.True(mTokenProvider.HasNoValidAccessToken());
        }

        [Fact]
        public void GetAccessToken()
        {
            SetValues("token", FUTURE_VALID_UNTIL, "28800000");

            // token which is stored, is valid
            Assert.Equal("token", mTokenProvider.GetAccessToken());
            Assert.False(mTokenProvider.HasNoValidAccessToken());
        }

        [Fact]
        public void GetAccessToken_WithExpiredToken()
        {
            SetValues("token", PAST_VALID_UNTIL, "28800000");

            Assert.Null(mTokenProvider.GetAccessToken());
            Assert.True(mTokenProvider.HasNoValidAccessToken());
        }

        [Fact]
        public void GetAccessToken_WithMissingField()
        {
            SetValues("token", PAST_VALID_UNTIL, "28800000");

            Assert.Null(mTokenProvider.GetAccessToken());
            Assert.True(mTokenProvider.HasNoValidAccessToken());
        }

        private void SetValues(String token, String validUntil, String duration)
        {
            Set(FieldName.ACCESS_TOKEN, token);
            Set(FieldName.ACCESS_TOKEN_VALID_UNTIL, validUntil);
            Set(FieldName.ACCESS_TOKEN_DURATION, duration);
        }
    }
}
