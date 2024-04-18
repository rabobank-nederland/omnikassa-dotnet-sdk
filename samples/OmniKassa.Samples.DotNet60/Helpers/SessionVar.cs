using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace example_dotnet60.Helpers
{
    public abstract class SessionVar
    {
        public static T Get<T>(ISession session, string key)
        {
            if (session.Get(key) == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(session.GetString(key));
            }
        }

        public static void Set<T>(ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
