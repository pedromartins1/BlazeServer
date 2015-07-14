using System.IO;

using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace BlazeServer
{
    public class Configuration
    {
        public struct Config
        {
            
        }

        public static void Load(string filename)
        {
            var buffer = File.ReadAllText(filename);

            var deserializer = new Deserializer();
            var config = deserializer.Deserialize<Config>(new StringReader(buffer));

            
        }
    }
}
