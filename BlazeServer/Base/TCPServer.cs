using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace BlazeServer
{
    public abstract class TCPServer
    {
        public TcpListener _server;
        public Boolean _isRunning;
        public ushort _port;
        public Thread _thread;
        public uint _connections;
        public IPEndPoint _endPoint;

        public Socket _listener;

        public TCPServer(ushort port)
        {
            _port = port;
            _thread = new Thread(new ThreadStart(LoopClients));
            _endPoint = new IPEndPoint(IPAddress.Any, _port);
            _connections = 0;

            _listener = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            _server = new TcpListener(_endPoint);
            _server.Start();

            Log.Info(string.Format("Listening on {0}.", _endPoint.ToString()));

            _isRunning = true;
            _thread.Start();
        }

        public void LoopClients()
        {
            //Log.Debug("Waiting for client connections...");

            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();

                // client found.
                // create a thread to handle communication
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public abstract void HandleClient(object obj);
    }
}