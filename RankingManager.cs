using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net.Http;

namespace CsharpRanking
{
    /// <summary>
    /// ランキングデータを管理するクラス
    /// 外部サーバーへのランキング機能を提供
    /// 接続失敗時ローカルのデータベースを利用する
    /// </summary>
    public class RankingManager
    {
        //= "http://localhost/runking/GetData.php";

        public static OrderType Oder { private set; get; }
        public static bool CanOnline { private set; get; }

        public string BaseUrl { private set; get; }

        private string ConfigFilePath = ConfigPath.LocalUserAppDataPath + "/config.txt";

        private const string GET_DATA_URL = "/runking/GetData.php";

        SQLite.SQLite s = new SQLite.SQLite();

        public RankingManager(string gamename, UInt64 gameid, ScoreType scoreType, OrderType orderType)
        {
            if (!RankingData.SetGameID(gameid)) throw new System.ArgumentOutOfRangeException("Game ID is out of range", "gameid");
            RankingData.SetScoreType(scoreType);
            RankingManager.Oder = orderType;
            SQLite.SQLite.SetGameName(gamename);
        }

        /// <summary>
        /// 外部データベースに接続, 初期設定
        /// </summary>
        /// <returns>true:接続成功, false:接続失敗</returns>
        public bool Init()
        {
            s.ConnectionOpen();
            s.CreateTable();
            s.ConnectionClose();
            if (this.LoadServerAdress())
            {
#if DEBUG
                Console.WriteLine("Success for address read");
#endif
                RankingManager.CanOnline = true;
                return true;
            }
            else
            {
#if DEBUG
                Console.WriteLine("Failed to address read");
#endif
                RankingManager.CanOnline = false;
                return false;
            }

        }
        /// <summary>
        /// 新規データをセットして最新ランキングを取得
        /// </summary>
        public void DataSetAndLoad<Type>(Type data, string dataName = "")
            where Type : struct
        {
            RankingData newdata = new RankingData(data.ToString(), dataName);

            s.ConnectionOpen();
            s.InsertRecord(newdata);
            s.ConnectionClose();
            
            if (CanOnline) {
                NameValueCollection nvc = new NameValueCollection();
                //Console.WriteLine(this.SaveAndGetData(newdata));
                try
                {
                    var task = Task.Run(() =>
                    {
                        return this.SaveAndGetData(newdata);
                    });
#if DEBUG
                    System.Console.WriteLine(task.Result);
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
#endif
            }
                return;
        }
        
        /// <summary>
        /// 外部データベースからデータ取得
        /// </summary>
        public void GetData()
        {
            s.ConnectionOpen();
            s.SelectRecord();
            s.ConnectionClose();
        }

        private async Task<string> SaveAndGetData(RankingData data)
        {
            var content = new System.Net.Http.FormUrlEncodedContent(data.Dictionary());
            var client = new System.Net.Http.HttpClient();
#if DEBUG
            Console.WriteLine(content);
#endif
            var response = await client.PostAsync(BaseUrl + GET_DATA_URL, content);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// ローカルにあるサーバーアドレス情報読み込み
        /// </summary>
        /// <returns>true:読み込み成功, false:読み込み失敗</returns>
        private bool LoadServerAdress()
        {
            System.IO.StreamReader sr;

            try
            {
                sr = System.IO.File.OpenText(ConfigFilePath);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Console.WriteLine("File not fount.");
                System.Console.WriteLine(ex.Message);
                return false;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                System.Console.WriteLine("Access denied.");
                System.Console.WriteLine(ex.Message);
                return false;
            }

            this.BaseUrl = sr.ReadToEnd();
            sr.Close();
            return true;
        }
    }
}
