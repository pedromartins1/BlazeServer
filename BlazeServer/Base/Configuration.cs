using System.IO;

using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace BlazeServer
{
    public class Configuration
    {
        public struct Config
        {
            public DatabaseConfiguration DatabaseConfiguration { get; set; }

            public bool DebugLog { get; set; }
        }

        public static void Load(string filename)
        {
            var buffer = File.ReadAllText(filename);

            var deserializer = new Deserializer(ignoreUnmatched: true);
            var config = deserializer.Deserialize<Config>(new StringReader(buffer));

            DatabaseConfiguration = config.DatabaseConfiguration;
            DebugLog = config.DebugLog;
        }

        public static DatabaseConfiguration DatabaseConfiguration { get; set; }

        public static bool DebugLog { get; set; }
    }

    public class DatabaseConfiguration
    {
        public string Host { get; set; }

        public ushort Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }
    }
}
