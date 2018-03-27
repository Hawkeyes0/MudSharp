using System;
using Mud.Common;
using Mud.Common.Logging;

namespace Mud.Engine
{
    class Program
    {
        private static readonly Logger<Program> _logger = new Logger<Program>();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ushort port = 2121;
            if (args != null && args.Length == 1)
            {
                ushort.TryParse(args[0], out port);
            }

            LogLevel.Default = LogLevel.Debug;
            
            GameEngine engine = Singleton<GameEngine>.Instance;
            LogEngineInfo(engine);
            _logger.Info($"{engine.Name} MUD is running on port {port} now.");
            try{
                engine.Run(port);
            } catch(Exception e){
                _logger.Error("Unhandled exception be thrown.", e);
            }
            finally{
                engine.Close();
            }

            _logger.Info("The server shutdown.");
        }

        private static void LogEngineInfo(GameEngine engine)
        {
            var assembly = engine.GetType().Assembly;
            _logger.Info($@"Assembly: {assembly.GetName().Name}
Version: {assembly.GetName().Version}
Location: {assembly.Location}
Create date: {new System.IO.FileInfo(assembly.Location).LastWriteTime:yyyy-MM-dd HH:mm:ss.fff}");
        }
    }
}
