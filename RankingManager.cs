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
        private static OrderType Oder { set; get; }
        private static bool CanOnline { set; get; }
        private static bool IsOnline { set; get; }

        private string BaseUrl { set; get; }
        private static UInt64 limit = 5;

        private string ConfigFilePath = ConfigPath.LocalUserAppDataPath + "/config.txt";

        private const string GET_DATA_URL = "/ranking/GetData.php";
        private const string SAVE_DATA_URL = "/ranking/SaveData.php";

        SQLite.SQLite s = new SQLite.SQLite();

        /// <summary>
        /// コンストラクタ, ランキングマネージャーの初期設定
        /// </summary>
        /// <param name="gamename">ゲーム名を指定</param>
        /// <param name="gameid">ゲームのID</param>
        /// <param name="scoreType">ScoreType型, スコアデータのデータ型</param>
        /// <param name="orderType">OrderType型, スコアデータのソート順</param>
        /// <param name="onlie">手動オンライン設定, デフォルト:true</param>
        public RankingManager(string gamename, UInt64 gameid, ScoreType scoreType, OrderType orderType, bool onlie = true)
        {
            if (!RankingData.SetGameID(gameid))
            {
                Log.Fatal("Game ID is out of range.");
                throw new System.ArgumentOutOfRangeException("Game ID is out of range", "gameid");
            }
            Log.SetGameID(RankingData.GameID);
            RankingData.SetScoreType(scoreType);
            RankingManager.Oder = orderType;
            RankingManager.IsOnline = onlie;
            SQLite.SQLite.SetGameName(gameid);
            Log.Info("Instance was created.");
        }

        /// <summary>
        /// 外部データベースに接続, 初期設定
        /// </summary>
        /// <returns>true:接続成功, false:接続失敗</returns>
        public void Init()
        {
            Log.Info("RankingManager initialization start.");
            s.ConnectionOpen();
            s.CreateTable();
            s.ConnectionClose();
            if (this.LoadServerAddress() && RankingManager.IsOnline)
            {
                Log.Info("【SUCCESS】【File】Success for address read.");
                RankingManager.CanOnline = true;
            }
            else if (RankingManager.IsOnline)
            {
                Log.Warn("【FAILED】【File】Failed to address read.");
                RankingManager.CanOnline = false;
            }
            Log.Info("【SUCCESS】RankingManager initialization finish.");

        }
        /// <summary>
        /// 新規データをセットして最新ランキングを取得
        /// </summary>
        public void DataSetAndLoad<Type>(double data, string dataName = "")
            where Type : struct
        {
            RankingData newdata = new RankingData(data, dataName);

            SaveLocal(newdata);

            if (IsOnline && CanOnline)
            {
                SaveOnline(newdata);
                GetOnlineData();
            }
        }

        public List<Ranking.RankingData> GetData()
        {
            var getlist = new List<Ranking.RankingData>();
            if (IsOnline && CanOnline)
            {
                try
                {
                    getlist = this.GetOnlineData();
                }catch(Exception ex)
                {
                    Log.Warn("【FAILED】【Online】Connection to server failed. Change to offline.");
                    RankingManager.CanOnline = false;
                    getlist = this.GetLocalData();
                }
            }
            else
            {
                getlist = this.GetLocalData();
            }
            return getlist;
        }


        /// <summary>
        /// 外部データベースからデータ取得
        /// </summary>
        public List<Ranking.RankingData> GetOnlineData()
        {
            var r = new List<Ranking.RankingData>();
            Log.Info("【Onlie】Get Online start.");
            try
            {
                var task = Task.Run(() =>
                {
                    return this.SendOnlieGetData();
                });
                Log.Debug(task.Result);
                Log.Info("【SUCCESS】【Onlie】Get Online finish.");
                return r;
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
                            Log.Fatal(exNestedInnerException.Message);
                        }
                        exNestedInnerException = exNestedInnerException.InnerException;
                    }
                    while (exNestedInnerException != null);
                }
                throw;
            }
            catch (HttpRequestException ex)
            {
                Log.Fatal(ex.Message);
                throw;
            }
            catch (System.Net.WebException ex)
            {
                Log.Fatal(ex.Message);
                throw;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Log.Fatal(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Ranking.RankingData> GetLocalData()
        {
            s.ConnectionOpen();
            var list = s.SelectRecord(RankingManager.Oder);
            s.ConnectionClose();
            foreach(var s in list)
            {
                Log.Debug(string.Format("ID = {0,4}, TIME = {1,20}, Name = {2,10}, Score = {3,5:#.###}",
                    s.DataID,
                    s.SaveTime,
                    s.DataName,
                    s.ScoreValue
                    ));
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SaveOnline(RankingData data)
        {
            Log.Info("【Onlie】Get Online start.");
            try
            {
                var task = Task.Run(() =>
                {
                    return this.SendOnlineSaveData(data);
                });
                Log.Debug(task.Result);
                Log.Info("【SUCCESS】【Onlie】Get Online finish.");
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
                            Log.Fatal(exNestedInnerException.Message);
                        }
                        exNestedInnerException = exNestedInnerException.InnerException;
                    }
                    while (exNestedInnerException != null);
                }
                throw;
            }
            catch (HttpRequestException ex)
            {
                Log.Fatal(ex.Message);
                throw;
            }
            catch (System.Net.WebException ex)
            {
                Log.Fatal(ex.Message);
                throw;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Log.Fatal(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newdata"></param>
        public void SaveLocal(RankingData newdata)
        {
            s.ConnectionOpen();
            s.InsertRecord(newdata);
            s.ConnectionClose();
        }

        private async Task<string> SendOnlineSaveData(RankingData data)
        {
            var content = new System.Net.Http.FormUrlEncodedContent(data.Dictionary());
            var client = new System.Net.Http.HttpClient();

            Log.Info("【Onlie】Access to server.");
            var response = await client.PostAsync(BaseUrl + SAVE_DATA_URL, content);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> SendOnlieGetData()
        {
            Dictionary<string, string> postid =  new Dictionary<string, string>
                    {
                        { "GameID", RankingData.GameID.ToString() }
                    };
            var content = new System.Net.Http.FormUrlEncodedContent(postid);
            var client = new System.Net.Http.HttpClient();

            Log.Info("【SUCCESS】【Onlie】Access to server.");
            var response = await client.PostAsync(BaseUrl + GET_DATA_URL, content);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// ローカルにあるサーバーアドレス情報読み込み
        /// </summary>
        /// <returns>true:読み込み成功, false:読み込み失敗</returns>
        private bool LoadServerAddress()
        {
            System.IO.StreamReader sr = null;
            Log.Info("【File】Access to local server address start.");
            try
            {
                sr = System.IO.File.OpenText(ConfigFilePath);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
            catch (NotSupportedException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
            catch (ArgumentException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
            catch (System.IO.PathTooLongException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                Log.Warn(ex.Message);
                return false;
            }
            Log.Info("【SUCCESS】【File】Access to local server address finish.");

            this.BaseUrl = sr.ReadToEnd();
            sr.Close();
            return true;
        }

        public void SetLimit(UInt64 l)
        {
            SQLite.SQLite.SetLimit(l);
            RankingManager.limit = l;
            Log.Info("【SUCCESS】Set limit is" + l.ToString());
            return;
        }
    }
}
