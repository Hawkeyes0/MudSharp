using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Mud.Common.Logging
{
    public sealed class Logger<T>
    {
        private string _clsName;
        private string _assembly;
        private int _level = LogLevel.Default;

        private static object locker = new object();

        public Logger()
        {
            var type = typeof(T);
            _clsName = type.FullName;
            _assembly = type.Assembly.FullName.Split(',')[0];
        }

        public void Error(string message, Exception ex = null, params object[] args)
        {
            if (_level > LogLevel.Error)
                return;

            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            WriteLogEntry(new LogEntry
            {
                Message = message,
                Exception = ex,
                Level = LogLevel.Error
            });
        }

        public void Warning(string message, Exception ex = null, params object[] args)
        {
            if (_level > LogLevel.Warning)
                return;

            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            WriteLogEntry(new LogEntry
            {
                Message = message,
                Exception = ex,
                Level = LogLevel.Warning
            });
        }

        public void Info(string message, Exception ex = null, params object[] args)
        {
            if (_level > LogLevel.Info)
                return;

            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            WriteLogEntry(new LogEntry
            {
                Message = message,
                Exception = ex,
                Level = LogLevel.Info
            });
        }

        public void Trace(string message, Exception ex = null, params object[] args)
        {
            if (_level > LogLevel.Trace)
                return;

            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            WriteLogEntry(new LogEntry
            {
                Message = message,
                Exception = ex,
                Level = LogLevel.Trace
            });
        }

        public void Debug(string message, Exception ex = null, params object[] args)
        {
            if (_level > LogLevel.Debug)
                return;

            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            WriteLogEntry(new LogEntry
            {
                Message = message,
                Exception = ex,
                Level = LogLevel.Debug
            });
        }

        private void WriteLogEntry(LogEntry logEntry)
        {
            string logfile = $"{_assembly}_{logEntry.Time:yyyyMMdd}.log";
            lock (locker)
            {
                File.AppendAllText(logfile, logEntry.ToString());
                Console.WriteLine(logEntry.ToString());
            }
        }
    }

    public sealed class LogEntry
    {
        public string Message { get; set; }

        public int Level { get; set; }

        public Exception Exception { get; set; }

        public DateTime Time { get; } = DateTime.Now;

        public override string ToString()
        {
            string msg = $@"[{Time:yyyy-MM-dd HH:mm:ss.fff}]-[{LogLevel.ToString(Level)}]-({GenerateName()}):
{Message}
";
            Exception e = Exception;
            while (e != null)
            {
                msg += e.Message + "\r\n";
                msg += e.StackTrace + "\r\n";
                e = e.InnerException;
            }

            return msg;
        }

        private string GenerateName()
        {
            StackTrace trace = new StackTrace(true);
            StackFrame frame = null;
            
            int i = 0;
            do{
                frame = trace.GetFrame(i);
                i++;
            }while(i<trace.FrameCount && frame.GetMethod().DeclaringType.Namespace == "Mud.Common.Logging");
            return $"{frame.GetFileName()}:line {frame.GetFileLineNumber()}";
        }
    }
}