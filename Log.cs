using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace CsharpRanking
{
    class Log
    {
        private static string FilePath = ConfigPath.LocalUserAppDataPath + "RankingLog.log";

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
            //yyyy-MM-dd HH:mm:ss 【xxxxx】 
            StackTrace st = new StackTrace(1, true);
            string fullname = st.GetFrame(1).GetMethod().ReflectedType.FullName;
            string methodname = st.GetFrame(1).GetMethod().Name;
            string lineNumber = st.GetFrame(0).GetFileLineNumber().ToString();
            string contents = string.Format("{0} [{1,5}] [{2}.{3}() line: {4,3}] - {5}",
                GetTime,
                level.ToString(),
                fullname,
                methodname,
                lineNumber,
                msg
            );
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    sw.WriteLine(contents);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static string GetTime {
            get {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");

            }
        }
    }
}

