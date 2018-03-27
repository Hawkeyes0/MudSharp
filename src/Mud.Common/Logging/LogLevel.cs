using System;

namespace Mud.Common.Logging
{
    public class LogLevel
    {
        public const int Debug = 0;
        public const int Trace = 1;
        public const int Info = 2;
        public const int Warning = 3;
        public const int Error = 4;

        public static int Default { get; set; } = Info;

        internal static string ToString(int level)
        {
            switch (level)
            {
                case Debug:
                    return "Debug";
                case Trace:
                    return "Trace";
                case Info:
                    return "Info";
                case Warning:
                    return "Warning";
                case Error:
                    return "Error";
                default:
                    return "Unknown";
            }
        }
    }
}