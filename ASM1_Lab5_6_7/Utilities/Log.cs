using System;
using NUnit.Framework;

namespace ASM.Utilities
{
    public static class Log
    {
        public static void Info(string message)
        {
            TestContext.Progress.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [INFO] {message}");
        }

        public static void Error(string message)
        {
            TestContext.Progress.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {message}");
        }
    }
}
