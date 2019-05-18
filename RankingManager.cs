using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

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
        private static Boolean CanOnline { set; get; }
        private static Boolean IsOnline { set; get; }

        private String BaseUrl { set; get; }
        public static UInt64 limit { private set; get; } = 5;

        private readonly String ConfigFilePath = ($"{Path.LocalPath}/config.txt");
        private const String ConfigDefaultSetting = "0";

        private const String GET_DATA_URL = "/ranking/GetData.php";
        private const String SAVE_DATA_URL = "/ranking/SaveData.php";

        SQLite.SQLite s = null;
        public readonly String Version = "3.2.0.0";

        /// <summary>
        /// ログパス
        /// </summary>
        /// <returns>ログのパス</returns>
        public String GetLogPath
        {
            get {
                return Log.GetFilePath;
            }
        }

        /// <summary>
        /// コンストラクタ, ランキングマネージャーの初期設定
        /// </summary>
        /// <param name="gamename">ゲーム名を指定</param>
        /// <param name="gameid">ゲームのID</param>
        /// <param name="orderType">OrderType型, スコアデータのソート順</param>
        /// <param name="onlie">手動オンライン設定, デフォルト:true</param>
        public RankingManager(String gamename, UInt64 gameid, OrderType orderType, Boolean onlie = true)
        {
            Log.Info($"【Start】[{gamename}]------------------------------------------------------------------------------------------" +
                "------------------------------------------------------------------------------------------");
            Log.Info($"Version = [{this.Version}].");
            if (!RankingData.SetGameID(gameid))
            {
                Log.Fatal("Game ID is out of range.");
                throw new System.ArgumentOutOfRangeException("Game ID is out of range", "gameid");
            }
            Log.SetGameID(RankingData.GameID);
            RankingManager.Oder = orderType;
            RankingManager.IsOnline = onlie;
            SQLite.SQLite.SetGameName(gameid);
            Log.Info("Instance was created.");

        }

        /// <summary>
        /// 外部データベースに接続, 初期設定
        /// </summary>
        public void Init()
        {
            Boolean success = false;
            s = new SQLite.SQLite();
            Log.Info("RankingManager initialization start.");
            s.ConnectionOpen();
            s.CreateTable();
            s.ConnectionClose();
            RankingManager.CanOnline = this.LoadServerAddress();

            if (RankingManager.CanOnline && RankingManager.IsOnline)
            {
                Log.Info("【Success】【File】Success for address read.");

                Log.Info("【Diff】Check Diff DataBase.");
                s.ConnectionOpen();
                List<Ranking.RankingData> data = null;
                data = s.DiffAllSelectRecord();
                Log.Info($"【Diff】diff database count = [{data.Count.ToString()}].");
                if (data.Count != 0)
                {
                    Log.Info("【Diff】diff database is exist.");
                    Log.Info("【Diff】diff database start transmission.");

                    foreach (var d in data)
                    {
                        success = this.SaveOnline(d);
                    }
                    Log.Info("【Success】【Diff】diff database transmission.");
                    if (success)
                    {
                        s.DiffAllDelete();
                    }
                }
                else
                {
                    Log.Info("【Diff】diff database is not exist.");
                }
                s.ConnectionClose();
            }
            else if (RankingManager.IsOnline)
            {
                Log.Warn("【FAILED】【File】Failed to address read.");
                RankingManager.CanOnline = false;
                Log.Info("RankingManager offline mode.");
            }
            Log.Info("【Success】RankingManager initialization finish.");
        }
        
        /// <summary>
        /// スコア, データ名を指定してセーブ。その後データ取得。
        /// </summary>
        /// <param name="data">double型:スコアデータ</param>
        /// <param name="dataName">string型:データ名</param>
        /// <returns>取得したランキングデータ型のリスト</returns>
        public List<RankingData> DataSetAndLoad(Double data, String dataName = "")
        {
            RankingData newdata = new RankingData(data, dataName);
            this.Save(newdata);

            return this.GetData();
        }

        /// <summary>
        /// ランキングデータ型でセーブ。その後データ取得
        /// </summary>
        /// <param name="data">RankingData型:セーブするランキングデータ</param>
        /// <returns>取得したランキングデータ型のリスト</returns>
        public List<RankingData> DataSetAndLoad(RankingData data)
        {
            this.Save(data);

            return this.GetData();
        }

        /// <summary>
        /// スコア, データ名を指定してセーブ。
        /// </summary>
        /// <param name="data">double型:スコアデータ</param>
        /// <param name="dataName">string型:データ名</param>
        public void SaveData(Double data, String dataName = "")
        {
            RankingData newdata = new RankingData(data, dataName);
            this.Save(newdata);
            return;
        }

        /// <summary>
        /// ランキングデータ型でセーブ。
        /// </summary>
        /// <param name="data">RankingData型:セーブするランキングデータ</param>
        public void SaveData(RankingData data)
        {
            this.Save(data);
        }

        private void Save(RankingData data)
        {
            Log.Info("Record Write start.");
            SaveLocal(data);
            Boolean success = false;
            if (IsOnline && CanOnline)
            {
                success =  SaveOnline(data);
            }
            else if (IsOnline && !CanOnline)
            {
                Log.Info("【Diff】Local diff save start.");
                s.ConnectionOpen();
                s.DiffInsertRecord(data);
                s.ConnectionClose();
                Log.Info("【Success】【Diff】Successful Local diff save.");
            }

            if (!success)
            {
                Log.Warn("【FAILED】【Online】Connection to server failed. Change to offline.");

                Log.Info("【Diff】Local diff save start.");
                s.ConnectionOpen();
                s.DiffInsertRecord(data);
                s.ConnectionClose();
                Log.Info("【Success】【Diff】Successful Local diff save.");

                RankingManager.CanOnline = false;
            }

        }

        /// <summary>
        /// ランキングデータ取得。オンラインに失敗した場合オフラインモードに移行。
        /// </summary>
        /// <returns>取得したランキングデータ型のリスト</returns>
        public List<RankingData> GetData()
        {
            Log.Info("Record acquisition start.");
            var getlist = new List<Ranking.RankingData>();
            if (IsOnline && CanOnline)
            {
                try
                {
                    getlist = this.GetOnlineData();
                }
                catch (Exception ex)
                {
                    Log.Warn(ex.Message);
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
        /// オンラインデータベースから全データ取得。
        /// 時間がかかる場合があるので使用しないことを推奨。
        /// </summary>
        /// <returns>取得したランキングデータ型のリスト</returns>
        public List<RankingData> GetAllData()
        {
            Log.Info("All record acquisition start.");
            var getlist = new List<Ranking.RankingData>();
            if (IsOnline && CanOnline)
            {
                try
                {
                    getlist = this.GetOnlineData(true);
                }
                catch (Exception ex)
                {
                    Log.Warn(ex.Message);
                    Log.Warn("【FAILED】【Online】Connection to server failed. Change to offline.");
                    RankingManager.CanOnline = false;
                    getlist = this.GetLocalAllData();
                }
            }
            else
            {
                getlist = this.GetLocalAllData();
            }
            Log.Info("All record acquisition end.");
            return getlist;
        }

        /// <summary>
        /// オンラインデータベースにデータ取得コマンド送信
        /// </summary>
        /// <returns>取得したランキングデータ型のリスト</returns>
        private List<RankingData> GetOnlineData(Boolean isAll = false)
        {
            var r = new List<Ranking.RankingData>();
            Log.Info("【Online】Get Online start.");
            try
            {
                var task = Task.Run(() =>
                {
                    return this.SendOnlieGetData(isAll);
                });
                Log.Debug($"【Server】{task.Result}");
                r = JsonConvert.DeserializeObject<List<RankingData>>(task.Result);
                foreach (var s in r)
                {
                    Log.Debug($"【Online】{s.ToString()}");
                }
                Log.Info("【Success】【Online】Get Online Success.");
                return r;
            }
            catch (AggregateException ex)
            {
                foreach (Exception e in ex.Flatten().InnerExceptions)
                {
                    Exception exNestedInnerException = e;
                    do
                    {
                        if (!String.IsNullOrEmpty(exNestedInnerException.Message))
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
            catch (ArgumentException ex)
            {
                Log.Fatal(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// ローカルデータベースからランキングデータ取得
        /// </summary>
        private List<RankingData> GetLocalData()
        {
            s.ConnectionOpen();
            var list = s.SelectRecord(RankingManager.Oder);
            s.ConnectionClose();
            foreach(var s in list)
            {
                Log.Debug($"【Local】{s.ToString()}");
            }
            return list;
        }

        /// <summary>
        /// ローカルデータベースから全ランキングデータ取得
        /// </summary>
        private List<Ranking.RankingData> GetLocalAllData()
        {
            s.ConnectionOpen();
            var list = s.AllSelectRecord();
            s.ConnectionClose();
            foreach (var s in list)
            {
                Log.Debug($"【Local】{s.ToString()}");
            }
            return list;
        }

        /// <summary>
        /// オンラインデータベースにデータ送信
        /// </summary>
        /// <param name="data">RankingData型:送信するランキングデータ</param>
        /// <returns>true:送信成功, false:送信失敗</returns>
        private bool SaveOnline(RankingData data)
        {

            Log.Info("【Online】Save Online start.");
            try
            {
                var task = Task.Run(() =>
                {
                    return this.SendOnlineSaveData(data);
                });
                Log.Debug(task.Result);
                Log.Info("【Success】【Online】Save Online Success.");
                return true;
            }
            catch (AggregateException ex)
            {
                foreach (Exception e in ex.Flatten().InnerExceptions)
                {
                    Exception exNestedInnerException = e;
                    do
                    {
                        if (!String.IsNullOrEmpty(exNestedInnerException.Message))
                        {
                            Log.Fatal(exNestedInnerException.Message);
                        }
                        exNestedInnerException = exNestedInnerException.InnerException;
                    }
                    while (exNestedInnerException != null);
                }
                return false;
            }
            catch (HttpRequestException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
            catch (System.Net.WebException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
            catch(ArgumentException ex)
            {
                Log.Fatal(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// ローカルデータベースに取得
        /// </summary>
        /// <param name="newdata"></param>
        private void SaveLocal(RankingData newdata)
        {
            Log.Info("Local save start.");
            s.ConnectionOpen();
            s.InsertRecord(newdata);
            s.ConnectionClose();
            Log.Info("【Success】Successful Local save.");
        }

        /// <summary>
        /// オンラインに非同期でセーブコマンドを送信するメソッド
        /// </summary>
        /// <param name="data">RankigData型:送信するランキングデータ</param>
        /// <returns>実行したSQLコマンド, もしくはサーバーから返信されたエラーメッセージ</returns>
        private async Task<string> SendOnlineSaveData(RankingData data)
        {
            var content = new System.Net.Http.FormUrlEncodedContent(data.Dictionary());
            var client = new System.Net.Http.HttpClient();

            Log.Info("【Online】Access to server.");
            Log.Info($"【Online】Address is [{BaseUrl}{SAVE_DATA_URL}].");
            Log.Info($"【Online】【Transmission】Ranking Data [{data.ToString()}].");
            Log.Info($"【Online】【Transmission】Contents [{ content.ToString()}].");
            var response = await client.PostAsync(BaseUrl + SAVE_DATA_URL, content);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// オンラインから非同期でランキングデータを取得するメソッド
        /// </summary>
        /// <returns>サーバーから受信したランキングデータをJSON化したもの, もしくはサーバーから返信されたエラーメッセージ</returns>
        private async Task<string> SendOnlieGetData(bool isAll = false)
        {

            String lim;
            if(isAll)
            {
                lim = 0.ToString();
            }
            else
            {
                lim = RankingManager.limit.ToString();
            }

            Dictionary<String, String> postid =  new Dictionary<String, String>
                    {
                        { "GameID", RankingData.GameID.ToString() },
                        { "OrderType", RankingManager.Oder.ToString() },
                        { "Limit", lim }
                    };
            var content = new System.Net.Http.FormUrlEncodedContent(postid);
            var client = new System.Net.Http.HttpClient();

            Log.Info("【Online】Access to server.");
            Log.Info($"【Online】Address is [{BaseUrl}{GET_DATA_URL}].");
            foreach (KeyValuePair<String, String> pair in postid)
            {
               Log.Info($"【Online】【Transmission】Contents key = [{pair.Key}], value = [{pair.Value}].");
            }
            Log.Info($"【Online】【Transmission】Contents [{content.Headers.ContentDisposition}].");
            var response = await client.PostAsync(BaseUrl + GET_DATA_URL, content);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// ローカルにあるサーバーアドレス情報読み込み
        /// </summary>
        /// <returns>true:読み込み成功, false:読み込み失敗,　オフラインモード</returns>
        private Boolean LoadServerAddress()
        {
            System.IO.StreamReader sr = null;
            System.IO.StreamWriter sw = null;
            Log.Info("【File】Access to local server address start.");
            try
            {
                sr = System.IO.File.OpenText(ConfigFilePath);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Log.Fatal(ex.Message);
                try
                {
                    Log.Info("【File】Create Setting File.");
                    using (sw = new System.IO.StreamWriter(ConfigFilePath, true, System.Text.Encoding.UTF8))
                    {
                        sw.Write(ConfigDefaultSetting);
                        Log.Info($"【File】Write[{ConfigDefaultSetting}]");
                    }
                }
                catch (ArgumentException exx)
                {
                    Log.Warn(exx.Message);
                }
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
            Log.Info("【Success】【File】Access to local server address finish.");

            this.BaseUrl = sr.ReadToEnd();
            Log.Info($"【File】BaseUrl[{this.BaseUrl}].");
            sr.Close();

            if(this.BaseUrl == "0")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取得するデータの個数
        /// default = 5
        /// </summary>
        /// <param name="lim">取得するデータ個数</param>
        public void SetLimit(UInt64 lim)
        {
            SQLite.SQLite.SetLimit(lim);
            RankingManager.limit = lim;
            Log.Info($"【Success】Set limit is {lim.ToString()}");
            return;
        }
    }
}
