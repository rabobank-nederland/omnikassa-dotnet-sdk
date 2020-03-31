using System;
using OmniKassa.Model;
using OmniKassa.Utils;

namespace OmniKassa.Tests.Model
{
    public class AccessTokenBuilder
    {
        private String token = "token";
        private DateTime validUntil = DateTimeUtils.StringToDate("2016-09-22T10:10:04.848+" + TestHelper.GetLocalTimeZone(""));
        private int durationInMillis = 1000 * 60 * 60 * 8;

        public AccessTokenBuilder WithToken(String token)
        {
            this.token = token;
            return this;
        }

        public AccessTokenBuilder WithValidUntil(DateTime validUntil)
        {
            this.validUntil = validUntil;
            return this;
        }

        public AccessTokenBuilder WithValidUntilTomorrow()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(1);
            return WithValidUntil(dateTime);
        }

        public AccessTokenBuilder WithValidUntilNow()
        {
            return WithValidUntil(DateTime.Now);
        }

        public AccessTokenBuilder WithDurationInMillis(int durationInMillis)
        {
            this.durationInMillis = durationInMillis;
            return this;
        }

        public AccessToken Build()
        {
            return new AccessToken(token, validUntil, durationInMillis);
        }
    }
}
