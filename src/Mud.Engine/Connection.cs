using System;
using System.Net.Sockets;
using System.Text;
using Mud.Common.Logging;

namespace Mud.Engine
{
    public class Connection
    {
        private readonly Socket _socket;
        private DateTime _lastActiveTime;
        private static readonly Logger<Connection> _logger = new Logger<Connection>();

        public Connection(Socket socket)
        {
            _socket = socket;
        }

        public string Host => _socket.RemoteEndPoint.ToString();

        public bool InUse => _socket.Connected && (DateTime.Now - _lastActiveTime).TotalMinutes > 20;

        internal void WriteLine(string input)
        {
            var buffer = Encoding.UTF8.GetBytes(input);
            _socket.Send(buffer);
        }

        internal string Read()
        {
            byte[] buffer = new byte[2048];
            int count = buffer.Length;
            StringBuilder builder = new StringBuilder();

            while (count == buffer.Length)
            {
                try
                {
                    count = _socket.Receive(buffer);
                }
                catch (SocketException e)
                {
                    if (e.ErrorCode == 10054)
                    {
                        _logger.Warning($"Connection close by remote {Host}.");
                        Close();
                        throw new Exception($"Connection close by remote {Host}.", e);
                    }
                    else
                    {
                        _logger.Warning("Socket exception error.", e);
                        throw new Exception("Socket exception error.", e);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("Exception thrown.", e);
                    throw new Exception("Exception thrown.", e);
                }
                if (count == 0)
                {
                    _logger.Trace("EOF.");
                    return string.Empty;
                }
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            return builder.ToString();
        }

        internal void Close()
        {
            _socket.Close();
        }
    }
}