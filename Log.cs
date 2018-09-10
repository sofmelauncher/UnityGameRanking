using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Ranking
{
    class Log
    {
        private static string FilePath = ConfigPath.LocalUserAppDataPath + "\\" +  DateTime.Now.ToString("yyyy-MM-dd") + "RankingLog.log";

        public static void Fatal(string message)
        {
            Output(message, LogLevel.INFO);
        }

        public static void Warn(string message)
        {
            Output(message, LogLevel.WARN);
        }

        public static void Info(string message)
        {
            Output(message, LogLevel.INFO);
        }

        public static void Debug(string message)
        {
            Output(message, LogLevel.DEBUG);
        }
        private static void Output(string msg, LogLevel level)
        {
            //yyyy-MM-dd HH:mm:ss [xxxxx][xxxxx.xxxxx line: xxx] - xxx

            StackTrace st = new StackTrace(1, true);
            string name = st.GetFrame(1).GetMethod().ReflectedType.FullName + "." + st.GetFrame(1).GetMethod().Name;
            string lineNumber = st.GetFrame(1).GetFileLineNumber().ToString();
            string contents = string.Format("{0} [{1,-5}] [{2,35}() line: {3,3}] - {4}",
                GetTime,
                level.ToString(),
                name,
                lineNumber,
                msg
            );
            Console.WriteLine(contents);
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    sw.WriteLine(contents);
                }
            }
            catch(ArgumentException ex)
            {
                Log.Warn(ex.Message);
            }
        }

        private static string GetTime {
            get {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            }
        }
    }
}

