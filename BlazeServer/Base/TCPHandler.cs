using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlazeServer
{
    class TCPHandler
    {
        private int _port;

        private Socket _socket;
        private SocketAsyncEventArgs _acceptEventArgs;

        public TCPHandler(int port)
        {
            _port = port;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _acceptEventArgs = new SocketAsyncEventArgs();
        }

        public void Start()
        {
            Log.Info("Starting StreamServer");

            try
            {
                _acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(_acceptEventArgs_Completed);
                _socket.Bind(new IPEndPoint(IPAddress.Any, _port));
                _socket.Listen(1);

                DoAccept(_acceptEventArgs);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        private void DoAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            if (!_socket.AcceptAsync(args))
            {
                _acceptEventArgs_Completed(this, args);
            }
        }

        void _acceptEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success)
                {
                    Log.Info("yay");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            DoAccept(e);
        }
    }
}
