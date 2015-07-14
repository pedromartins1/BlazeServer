using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeniceSnowroller
{
    class Program
    {
        static void Log(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
        }

        static void LogState(string message)
        {
            Console.Clear();
            Console.WriteLine("[CURRENT STATE]");
            Console.WriteLine(message);
        }

        static void ReadPipeData()
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "venice_snowroller", PipeDirection.InOut, PipeOptions.None);

            if (pipeClient.IsConnected != true)
            {
                pipeClient.Connect();
            }

            try
            {
                using (StreamReader sr = new StreamReader(pipeClient))
                {
                    while (pipeClient.IsConnected)
                    {
                        char[] buffer = new char[64];

                        while (sr.Read(buffer, 0, buffer.Length) != 0)
                        {
                            //LogState(string.Format("{0} {1} {2} {3}", new string(buffer, 5, 12), new string(buffer, 19, 10), new string(buffer, 30, 8), new string(buffer, 39, 1)));
                            LogState(new string(buffer));
                        }
                    }

                    if (!pipeClient.IsConnected)
                    {
                        return;
                    }
                }
            }
            catch
            {

            }
        }

        static void Main(string[] args)
        {
            /* Log("Waiting for game...");

            Process bf3process = null;

            while (bf3process == null)
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes)
                {
                    if (process.ProcessName == "bf3")
                    {
                        bf3process = process;
                    }
                }
            } */

            bool waiting = false;

            while (true)
            {
                String[] listOfPipes = System.IO.Directory.GetFiles(@"\\.\pipe\");

                if (listOfPipes.Contains(@"\\.\pipe\venice_snowroller"))
                {
                    ReadPipeData();
                    waiting = false;
                }
                else
                {
                    if (!waiting)
                    {
                        Log("Waiting for pipe 'venice_snowroller'...");
                        waiting = true;
                    }
                }
            }

            Console.ReadKey();
        }
    }
}