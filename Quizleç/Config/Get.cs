using System.IO;
using System.Text.Json;

namespace Quizleç.Config
{
    public static class Get
    {
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
    }
}
