using System;

namespace ProjectFileAnalyzer
{
    /// <summary>
    /// Keep it simple please.
    /// </summary>
    public class Log
    {
        public static void WriteInfo(string data)
        {
            Console.WriteLine(data);
        }

        public static void WriteInfo(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public static void WriteWarning(string data)
        {
            Console.WriteLine(data);
        }

        public static void WriteWarning(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}