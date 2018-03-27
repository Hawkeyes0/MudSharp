using System;
using System.Collections.Generic;

namespace Mud.Engine
{
    public class ConnectionPool
    {
        private static List<string> _banList;

        internal static List<Connection> Connections{get;} = new List<Connection>();
        
        internal static bool IsBan(string remote)
        {
            return _banList.Contains(remote);
        }
    }
}