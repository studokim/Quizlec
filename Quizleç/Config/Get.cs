using System;
using System.IO;
using System.Text.Json;

namespace Quizleç.Config
{
    public static class Get
    {
        public static int CurrentIndex =
            (int)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - 1608700000000);
        public static AppSettings AppSettingsOptions(string environment = null)
        {
            // TODO: is it fine instead of Options pattern?
            string path = (environment == null) ?
                "appsettings.json" : "appsettings." + environment + ".json";
            var jsonString = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppSettings>(jsonString);
        }

        public static Aerospike AerospikeOptions(string environment = null)
        {
            return AppSettingsOptions(environment).Aerospike;
        }

        public static int Index()
        {
            // TODO: rewrite indexing, it's enough only for 1.5 months.
            return ++CurrentIndex;
        }
    }
}
