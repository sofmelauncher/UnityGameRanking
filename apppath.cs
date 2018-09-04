using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

public class ConfigPath
{
    public static string UserAppDataPath {
        get {
            return GetFileSystemPath(Environment.SpecialFolder.ApplicationData);
        }
    }

    public static string CommonAppDataPath {
        get {
            return GetFileSystemPath(Environment.SpecialFolder.CommonApplicationData);
        }
    }

    public static string LocalUserAppDataPath {
        get {
            return GetFileSystemPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }

    public static RegistryKey CommonAppDataRegistry {
        get {
            return GetRegistryPath(Registry.LocalMachine);
        }
    }

    public static RegistryKey UserAppDataRegistry {
        get {
            return GetRegistryPath(Registry.CurrentUser);
        }
    }


    private static string GetFileSystemPath(Environment.SpecialFolder folder)
    {
        // パスを取得
        string path = String.Format(@"{0}\{1}\{2}",
            Environment.GetFolderPath(folder),  // ベース・パス
            Application.CompanyName,            // 会社名
            Application.ProductName);           // 製品名

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


    private static RegistryKey GetRegistryPath(RegistryKey key)
    {
        // パスを取得
        string basePath;
        if (key == Registry.LocalMachine)
            basePath = "SOFTWARE";
        else
            basePath = "Software";
        string path = String.Format(@"{0}\{1}\{2}",
            basePath,                           // ベース・パス
            Application.CompanyName,            // 会社名
            Application.ProductName);           // 製品名

        // パスのレジストリ・キーの取得（および作成）
        return key.CreateSubKey(path);
    }
}