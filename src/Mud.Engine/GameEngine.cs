using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Mud.Common.Logging;

namespace Mud.Engine
{
    internal class GameEngine
    {
        private Socket _listener;
        private Socket _listenerv6;
        private Thread _tv4, _tv6;

        private static readonly Logger<GameEngine> _logger = new Logger<GameEngine>();

        public GameEngine()
        {
        }

        public object Name { get; internal set; }
        public bool Running { get; private set; }

        internal void Run(ushort port)
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, port));
            _listener.Listen(3);

            _listenerv6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            _listenerv6.Bind(new IPEndPoint(IPAddress.IPv6Any, port));
            _listenerv6.Listen(3);

            _tv4 = new Thread(AcceptConnection);
            _tv4.Start(_listener);
            _tv6 = new Thread(AcceptConnection);
            _tv6.Start(_listenerv6);
        }

        private void AcceptConnection(object input)
        {
            Socket listener = input as Socket;
            if (listener == null)
            {
                _logger.Error("Listener is null.");
                throw new ArgumentNullException(nameof(input), "Listener is null.");
            }

            while (Running)
            {
                Socket socket = listener.Accept();
                if (socket == null)
                {
                    throw new NullReferenceException("Accept a null socket.");
                }

                Connection conn = new Connection(socket);
                string remote = conn.Host;
                _logger.Info($"New connection: {remote}");
                if (ConnectionPool.IsBan(remote))
                {
                    conn.WriteLine("Your IP has been banned.");
                    conn.Close();
                    continue;
                }

                ConnectionPool.Connections.Add(conn);

                SystemInfo.MaxPlayers = Math.Max(++SystemInfo.NumPlayers, SystemInfo.MaxPlayers);
                if(SystemInfo.MaxPlayers > SystemInfo.MaxPlayersEver){
                    SystemInfo.MaxPlayersEver = SystemInfo.MaxPlayers;
                    SystemInfo.MaxPlayersEverTime = DateTime.Now;
                }
            }
        }

        internal void Close()
        {
            _listener.Close();
            _listenerv6.Close();
            _listener.Dispose();
            _listenerv6.Dispose();
        }
    }
}