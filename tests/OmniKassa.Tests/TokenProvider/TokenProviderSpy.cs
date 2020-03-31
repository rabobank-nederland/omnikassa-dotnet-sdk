using System;
using System.Collections.Generic;
using OmniKassa.Model;
using OmniKassa.Utils;

namespace OmniKassa.Tests.Connector
{
    public class TokenProviderSpy : TokenProvider
    {
        private Dictionary<FieldName, String> mMap;

        public TokenProviderSpy(Dictionary<FieldName, String> map)
        {
            mMap = map;
        }

        protected override String GetValue(FieldName key)
        {
            mMap.TryGetValue(key, out string value);
            return value;
        }

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

        public void SetValidAccessToken()
        {
            SetAccessToken(new AccessToken("test", GetTomorrow(), 12800));
        }

        public void SetAccessToken(String value)
        {
            SetValue(FieldName.ACCESS_TOKEN, value);
        }

        public void SetValidUntil(DateTime value)
        {
            SetValue(FieldName.ACCESS_TOKEN_VALID_UNTIL, DateTimeUtils.DateToString(value));
        }

        private DateTime GetTomorrow()
        {
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(1);
            return dateTime;
        }
    }
}
