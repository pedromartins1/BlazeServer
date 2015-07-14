using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    class BlazeServer
    {
        private static ushort ListenPort = 10041;

        public static void Start()
        {
            var listener = new TcpListener(IPAddress.Any, ListenPort);
            listener.Start();

            new Task(() =>
            {
                while (true)
                {
                    var client = listener.AcceptTcpClient();
                    new Task(() => AcceptConnection(client)).Start();
                }
            }).Start();

            Log.Info(string.Format("Listening on port {0}.", ListenPort));
        }

        private static void AcceptConnection(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();

                new Task(() => ReadFromClient(stream)).Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ReadFromClient(Stream stream)
        {
            var data = new byte[4096];

            while (true)
            {
                int clientBytes;

                try
                {
                    clientBytes = stream.Read(data, 0, data.Length);
                }
                catch
                {
                    break;
                }

                if (clientBytes == 0)
                {
                    break;
                }
                else
                {
                    Router.HandleRequest(data);
                }
            }
        }
    }
}
