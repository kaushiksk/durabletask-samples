using System;

namespace DtfTester
{
    public static class Logger
    {
        public static void Log(string source, string  message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[{0}] [{1}] {2}", DateTime.UtcNow.TimeOfDay.ToString("c"), source, message);
            Console.ResetColor();
        }
    }
}
