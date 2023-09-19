using System;

namespace DurableTaskSamples.Common.Logging
{
    public static class Logger
    {
        private static bool ShouldLogVerbose = true;

        public static void Log(string source, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[{0}] [{1}] {2}", DateTime.UtcNow.TimeOfDay.ToString("c"), source, message);
            Console.ResetColor();
        }

        public static void LogVerbose(string source, string message)
        {
            if (ShouldLogVerbose)
            {
                Log(source, message);
            }
        }

        public static void SetVerbosity(bool verbose)
        {
            ShouldLogVerbose = verbose;
        }
    }
}
