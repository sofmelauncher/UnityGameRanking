using System;
using System.IO;
using System.Windows.Forms;

namespace Ranking
{
    class Path
    {
        public static string LocalPath {
            get {
                return GetFileSystemPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }

        private static string GetFileSystemPath(Environment.SpecialFolder folder)
        {
            // パスを取得
            string path = String.Format(@"{0}\{1}\",
                Environment.GetFolderPath(folder),
                "Launcher2018");

            // パスのフォルダを作成
            lock (typeof(Application))
            {
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
            }
            return path;
        }


    }
}
