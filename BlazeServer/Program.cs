using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BlazeServer
{
    class Program
    {
        private async Task Start(string configFileName)
        {
            Log.Initialize("BlazeServer.log", LogLevel.All, true);
            Log.Info("Starting Blaze server...");

            try
            {
                Configuration.Load(configFileName ?? "config.yml");

                if (Configuration.DatabaseConfiguration == null)
                {
                    Log.Error("Database configuration was not found.");
                    return;
                }
            }
            catch (IOException)
            {
                Log.Error(string.Format("Could not open the configuration file {0}.", configFileName ?? "config.yml"));
                return;
            }

            Database.Initialize();
            ClientManager.Initialize();

            var blazeServer = new BlazeServer(10041);
            blazeServer.Start();

            var telemetryServer = new TelemetryServer(9988);
            telemetryServer.Start();

            var tickerServer = new TickerServer(8999);
            tickerServer.Start();

            /* new Thread(() =>
            {

            }).Start(); */

            while (true)
            {
                try
                {
                    Log.WriteAway();

                    ClientManager.CleanClients();
                }
                catch (Exception e) { Log.Error(e.ToString()); }

                Thread.Sleep(5000);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                // start the program
                new Program().Start((args.Length > 0) ? args[0] : null).Wait();

                Environment.Exit(0);
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.InnerException.ToString());

                Environment.Exit(1);
            }
        }
    }
}
