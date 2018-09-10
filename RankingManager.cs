using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net.Http;

namespace Ranking
{
    /// <summary>
    /// ランキングデータを管理するクラス
    /// 外部サーバーへのランキング機能を提供
    /// 接続失敗時ローカルのデータベースを利用する
    /// </summary>
    public class RankingManager
    {
        //= "http://localhost/runking/GetData.php";

        private static OrderType Oder { set; get; }
        private static bool CanOnline { set; get; }

        private string BaseUrl { set; get; }

        private string ConfigFilePath = ConfigPath.LocalUserAppDataPath + "/config.txt";

        private const string GET_DATA_URL = "/runking/GetData.php";

        SQLite.SQLite s = new SQLite.SQLite();

        /// <summary>
        /// コンストラクタ, ランキングマネージャーの初期設定
        /// </summary>
        /// <param name="gamename">ゲーム名を指定</param>
        /// <param name="gameid">ゲームのID</param>
        /// <param name="scoreType">ScoreType型, スコアデータのデータ型</param>
        /// <param name="orderType">OrderType型, スコアデータのソート順</param>
        public RankingManager(string gamename, UInt64 gameid, ScoreType scoreType, OrderType orderType)
        {
            if (!RankingData.SetGameID(gameid))
            {
                Log.Warn("Game ID is out of range.");
                throw new System.ArgumentOutOfRangeException("Game ID is out of range", "gameid");
            }
            RankingData.SetScoreType(scoreType);
            RankingManager.Oder = orderType;
            SQLite.SQLite.SetGameName(gameid);
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
                Log.Info("Success for address read.");
                RankingManager.CanOnline = true;
                return true;
            }
            else
            {
                Log.Info("Failed to address read.");
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

            if (CanOnline)
            {
                NameValueCollection nvc = new NameValueCollection();
                //Console.WriteLine(this.SaveAndGetData(newdata));
                try
                {
                    var task = Task.Run(() =>
                    {
                        return this.SaveAndGetData(newdata);
                    });
                    System.Console.WriteLine(task.Result);
                }
                catch (AggregateException ex)
                {
                    foreach (Exception e in ex.Flatten().InnerExceptions)
                    {
                        Exception exNestedInnerException = e;
                        do
                        {
                            if (!string.IsNullOrEmpty(exNestedInnerException.Message))
                            {
                                Log.Warn(exNestedInnerException.Message);
                            }
                            exNestedInnerException = exNestedInnerException.InnerException;
                        }
                        while (exNestedInnerException != null);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Log.Warn(ex.Message);
                }
                catch (System.Net.WebException ex)
                {
                    Log.Warn(ex.Message);
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Log.Warn(ex.Message);
                }
                return;
            }
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
            Console.WriteLine(content.ReadAsStringAsync());
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
            System.IO.StreamReader sr = null;

            try
            {
                sr = System.IO.File.OpenText(ConfigFilePath);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Log.Warn(ex.Message);
                return false;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Log.Warn(ex.Message);
                return false;
            }
            catch (NotSupportedException ex)
            {
                Log.Warn(ex.Message);
                return false;
            }
            catch (ArgumentException ex)
            {
                Log.Warn(ex.Message);
                return false;
            }
            catch (System.IO.PathTooLongException ex)
            {
                Log.Warn(ex.Message);
                return false;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                Log.Warn(ex.Message);
                return false;
            }

            this.BaseUrl = sr.ReadToEnd();
            sr.Close();
            return true;
        }
    }
}
