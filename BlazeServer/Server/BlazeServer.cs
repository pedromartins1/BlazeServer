using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace BlazeServer
{
    public class BlazeServer : TCPServer
    {
        public BlazeServer(ushort port)
            : base(port)
        {
            
        }

        public override void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;

            byte[] message = new byte[10000];

            X509Certificate2 certificate = new X509Certificate2(Certificate.blazeServer, "123456");
            SslStream stream = new SslStream(client.GetStream(), false);

            try
            {
                stream.AuthenticateAsServer(certificate, false, SslProtocols.Ssl3, false);
            }
            catch
            {
                client.Close();
            }

            Log.Info("Client connected to Blaze.");

            // increase number of connections
            _connections += 1;

            // add the client to dictionary
            ClientManager.AddClient(_connections, client.Client.RemoteEndPoint, stream);

            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = stream.Read(message, 0, message.Length);
                }
                catch
                {
                    Log.Info("Client disconnected from Blaze.");
                    client.Close();

                    var clientId = _connections;
                    var cl = ClientManager.GetClient(clientId);

                    Log.Info(string.Format("Deleting client {0} (disconnected).", clientId));

                    if (cl.type == ClientType.CLIENT_TYPE_DEDICATED_SERVER && cl.gameId != 0)
                    {
                        Log.Info("Deleting game from disconnected event");
                        Database.DeleteGame(cl.gameId);
                    }

                    ClientManager.clients.Remove(clientId);
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }
                else
                {
                    // decode the packet
                    FireFrame fireFrame = new FireFrame(message);
                    Packet packet = fireFrame.Decode();

                    Router.HandleRequest(_connections, packet, stream);
                }
            }

            client.Close();
        }
    }
}