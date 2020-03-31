using System;
using System.IO;
using Newtonsoft.Json;

namespace OmniKassa.Tests
{
    public abstract class TestHelper
    {
        public static T GetObjectFromJsonFile<T>(String file)
        {
            String fullName = TestConfig.RESOURCE_PREFIX + file;
            var assembly = typeof(TestHelper).Assembly;
            string[] names = assembly.GetManifestResourceNames();
            Stream stream = assembly.GetManifestResourceStream(fullName);

            JsonSerializer js = new JsonSerializer();

            T expected = default(T);
            using (StreamReader sr = new StreamReader(stream))
            using (JsonTextReader jsonTextReader = new JsonTextReader(sr))
            {
                expected = js.Deserialize<T>(jsonTextReader);
            }
            return expected;
        }

        public static String GetLocalTimeZone(String separator)
        {
            return "02:00";
        }
    }
}
