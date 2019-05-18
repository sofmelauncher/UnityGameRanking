using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Ranking
{
    class Log
    {
        private static UInt64 GameID { set; get; }
        private readonly static String FilePath = ($"{Path.LocalPath}\\{DateTime.Now.ToString("yyyy-MM-dd")}RankingLog.log");
        private readonly static String HtmlFilePath = ($"{Path.LocalPath}\\{DateTime.Now.ToString("yyyy-MM-dd")}RankingLog.html");

        private const String defaultStyle = "white-space:nowrap;margin:0em 0em 0em 0;font-size:18px;font-family:'Tahoma';";
        private const String msgStart = "<span style=\"color:#9c27b0;\">";
        private const String msgEnd = "</span>";
        private const String tagStart = "<span style=\"color:#225fd9;    font-weight: 600;\">";
        private const String tagEnd = "</span>";

        //private static readonly LogMode mode = LogMode.DEBUG;
        private static readonly LogMode mode = LogMode.RELEASE;

        public static String GetFilePath {
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

            String contents = "";
            String htmlcontents = "";
            String style = ($"style = \"{defaultStyle}\"");
            switch (mode)
            {
                case LogMode.DEBUG:
                    StackTrace st = new StackTrace(1, true);
                    String name = st.GetFrame(1).GetMethod().ReflectedType.FullName + "." + st.GetFrame(1).GetMethod().Name;
                    String lineNumber = st.GetFrame(1).GetFileLineNumber().ToString();
                    contents = ($"{GetTime} [{level.ToString(),-5}] [{Log.GameID,2}] [{name,40}() line: {lineNumber,3}] - {msg}");
                    Console.WriteLine(contents);
                    break;
                case LogMode.RELEASE:
                    contents = ($"{GetTime} [{level.ToString(),-5}] [{Log.GameID,2}] - {msg}");
                    msg = msg.Replace("[", $"[{msgStart}");
                    msg = msg.Replace("]", $"{msgEnd}]");
                    msg = msg.Replace("【", $"[{tagStart}");
                    msg = msg.Replace("】", $"{tagEnd}]");
                    htmlcontents = ($"<p class = \"{level.ToString()}\" {style} >{GetTime} [{LevelStyle(level),-5}] [{Log.GameID,2}] - {msg}</p>");
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

            try
            {
                using (StreamWriter sw = new StreamWriter(HtmlFilePath, true, Encoding.UTF8))
                {
                    sw.WriteLine(htmlcontents);
                }
            }
            catch (ArgumentException ex)
            {
                Log.Warn(ex.Message);
            }
        }

        private static String GetTime {
            get {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            }
        }
        public static void SetGameID(UInt64 id)
        {
            Log.GameID = id;
        }

        private static String LevelStyle(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.FATAL:
                    return $"<span style=\"color:red\">{level.ToString()}</span>";
                case LogLevel.WARN:
                    return $"<span style=\"color:#ff9800;\">{ level.ToString()}</span > ";
                case LogLevel.INFO:
                    return $"<span style=\"color:black;\">{level.ToString()}</span>";
                case LogLevel.DEBUG:
                    return $"<span style=\"color:green;\">{level.ToString()}</span>";
            }
            return "";
        }
    }
}

