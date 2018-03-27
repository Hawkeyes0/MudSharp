using System;

namespace Mud.Engine
{
    internal static class SystemInfo
    {
        internal static int MaxPlayers { get; set; }

        internal static int NumPlayers { get; set; }

        internal static int MaxPlayersEver { get; set; }

        internal static DateTime MaxPlayersEverTime { get; set; }
    }
}