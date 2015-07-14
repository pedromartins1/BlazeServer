using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Initialize("BlazeServer.log", LogLevel.All);
            Log.Info("Starting Blaze server...");

            /* try
            {
                Configuration.Load("config.yml");
            }
            catch (IOException)
            {
                Log.Error(string.Format("Could not open the configuration file {0}.", "config.yml"));
                return;
            } */

            // start BlazeHub (gosredirector) server
            BlazeHub.Start();

            // start Blaze server
            BlazeServer.Start();

            while (true)
            {

            }
        }
    }
}
