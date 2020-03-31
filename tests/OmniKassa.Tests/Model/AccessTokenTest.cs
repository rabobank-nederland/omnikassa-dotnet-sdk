using System;
using Newtonsoft.Json;
using OmniKassa.Model;
using OmniKassa.Utils;
using Xunit;

namespace OmniKassa.Tests.Model
{
    public class AccessTokenTest
    {
        [Fact]
        public void AccessToken()
        {
            AccessToken accessToken = new AccessTokenBuilder().Build();

            Assert.Equal("token", accessToken.Token);
            Assert.Equal(28800000, accessToken.DurationInMillis);
            string expected = "2016-09-22T10:10:04.848+" + TestHelper.GetLocalTimeZone("\\:");
            Assert.Equal(expected, DateTimeUtils.DateToString((DateTime)accessToken.ValidUntil));

            Assert.False(accessToken.IsNotExpired());
        }

        [Fact]
        public void AccessToken_StillValidTomorrow()
        {
            AccessToken accessToken = new AccessTokenBuilder().WithValidUntilTomorrow().Build();

            Assert.True(accessToken.IsNotExpired());
        }

        [Fact]
        public void AccessToken_AssumeSomeClockDrift()
        {
            AccessToken accessToken = new AccessTokenBuilder().WithValidUntilNow().Build();

            // if a access token is about to expire than we assume its expired, so we force a refresh
            Assert.True(accessToken.IsExpired());
        }

        [Fact]
        public void AccessToken_StillValidForFiveMinutes()
        {
            DateTime expiredInFiveMinutes = DateTime.Now;
            expiredInFiveMinutes = expiredInFiveMinutes.AddMinutes(5);

            AccessToken accessToken = new AccessTokenBuilder().WithValidUntil(expiredInFiveMinutes).Build();

            // if a access token is about to expire than we assume its expired, so we force a refresh
            Assert.True(accessToken.IsNotExpired());
        }

        [Fact]
        public void AccessToken_MalformedJson()
        {
            Assert.Throws<ArgumentException>(() => JsonConvert.DeserializeObject<AccessToken>("{ 'oken': 'secret', 'durationInMillis': 123, 'validUntil': '2016-09-22T10:10:04.848+0200' }"));
            Assert.Throws<ArgumentException>(() => JsonConvert.DeserializeObject<AccessToken>("{ 'token': 'secret', 'urationInMillis': 123, 'validUntil': '2016-09-22T10:10:04.848+0200' }"));
            Assert.Throws<ArgumentException>(() => JsonConvert.DeserializeObject<AccessToken>("{ 'token': 'secret', 'durationInMillis': 123, 'alidUntil': '2016-09-22T10:10:04.848+0200' }"));
        }

        [Fact]
        public void Constructor()
        {
            AccessToken accessToken = new AccessToken("token", PrepareDateTime(), 123);
        }

        [Fact]
        public void Constructor_TokenIsNull()
        {
            Assert.Throws<ArgumentException>(() => new AccessToken(null, PrepareDateTime(), 123));
        }

        [Fact]
        public void Constructor_TokenIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new AccessToken("", PrepareDateTime(), 123));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_DurationInMillisIsZero()
        {
            Assert.Throws<ArgumentException>(() => new AccessToken("token", PrepareDateTime(), 0));
        }

        private DateTime PrepareDateTime()
        {
            return DateTimeUtils.StringToDate("2016-07-28T12:58:50.205+0200");
        }
    }
}
