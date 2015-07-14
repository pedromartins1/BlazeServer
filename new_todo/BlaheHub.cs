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
    class BlazeHub
    {
        private static ushort ListenPort = 42127;

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
                var certificate = new X509Certificate2("gosredirector.ea.com.pfx", "123456");

                var stream = new SslStream(client.GetStream(), false);
                stream.AuthenticateAsServer(certificate, false, SslProtocols.Ssl3, false);

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
                    FireFrame fireframe = new FireFrame(data);
                    HandleRequest(fireframe);
                }
            }
        }

        private static void HandleRequest(FireFrame fireframe)
        {
            switch (fireframe.GetComponent())
            {
                case 0x5:
                    RedirectorComponent.HandleRequest(fireframe);
                    break;

                default:
                    Log.Warn(string.Format("Received an unhandled request: {0}::{1}.", fireframe.GetComponent(), fireframe.GetCommand()));
                    break;
            }
        }
    }
}
