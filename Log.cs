using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Ranking
{
    class Log
    {
        private static UInt64 GameID { set; get; }
        private readonly static string FilePath = Path.LocalPath + "\\" +  DateTime.Now.ToString("yyyy-MM-dd") + "RankingLog.log";

        //private static readonly LogMode mode = LogMode.DEBUG;
        private static readonly LogMode mode = LogMode.RELEASE;

        public static string GetFilePath {
            get {
                return Log.FilePath;
            }
        }
        public static void Fatal(string message)
        { 
            Output(message, LogLevel.FATAL);
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
            //yyyy-MM-dd HH:mm:ss [xxxxx][yyy][xxxxx.xxxxx line: xxx] - xxx

            string contents = "";
            switch (mode)
            {
                case LogMode.DEBUG:
                    StackTrace st = new StackTrace(1, true);
                    string name = st.GetFrame(1).GetMethod().ReflectedType.FullName + "." + st.GetFrame(1).GetMethod().Name;
                    string lineNumber = st.GetFrame(1).GetFileLineNumber().ToString();
                    contents = string.Format("{0} [{1,-5}] [{2,2}] [{3,40}() line: {4,3}] - {5}",
                        GetTime,
                        level.ToString(),
                        Log.GameID,
                        name,
                        lineNumber,
                        msg
                    );
                    Console.WriteLine(contents);
                    break;
                case LogMode.RELEASE:
                    contents = string.Format("{0} [{1,-5}] [{2,2}] - {3}",
                        GetTime,
                        level.ToString(),
                        Log.GameID,
                        msg
                    );
                    break;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    sw.WriteLine(contents);
                }
            }
            catch (ArgumentException ex)
            {
                Log.Warn(ex.Message);
            }
        }

        private static string GetTime {
            get {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            }
        }
        public static void SetGameID(UInt64 id)
        {
            Log.GameID = id;
        }
    }
}

