using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpRanking
{
    ///<summary>
    ///外部サーバーへのランキング機能を提供
    ///接続失敗時ローカルのデータベースを利用する
    ///</summary>
    public class RankingManager : IServer
    {
        //= "http://localhost/runking/GetData.php";

        public static OrderType Oder { private set; get; }
        public static bool CanOnline { private set; get; }

        public string BaseUrl { private set; get; }
        public string PostString { get; set; }
        public string ResData { private set; get; }

        private string PostData { get; set; }
        private Encoding enc = Encoding.GetEncoding("UTF-8");
        private string ConfigFilePath = ConfigPath.LocalUserAppDataPath + "/config.txt";

        private const string GET_DATA_URL = "/runking/GetData.php";


        public RankingManager()
        {

        }

        ///<summary>
        ///外部データベースに接続, 初期設定
        ///</summary>
        ///<returns>true:接続成功, false:接続失敗</returns>
        public bool Init(UInt64 gameid, ScoreType scoreType, OrderType orderType)
        {
            if (this.LoadServerAdress() && RankingData.SetGameID(gameid))
            {
                Console.WriteLine("Conection to server.");
                Score.SetScoreType(scoreType);
                RankingManager.Oder = orderType;
                RankingManager.CanOnline = true;
                return true;
            }
            else
            {
                RankingManager.CanOnline = false;
                return false;
            }

        }
        ///<summary>
        ///外部データベースからデータ取得
        ///</summary>
        public void GetData()
        {
            this.PostData = "id=1&word=" + System.Web.HttpUtility.UrlEncode(PostString, enc);

            System.Net.WebClient wc = new System.Net.WebClient();
            wc.Encoding = enc;

            wc.Headers.Add("Content-Type", "application/plain");
            try
            {
                ResData = wc.UploadString(BaseUrl + GET_DATA_URL, PostData);
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine("Connection to server failed.");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine(PostData);
            wc.Dispose();

            Console.WriteLine("data：");
            Console.WriteLine(ResData);
        }

        public void SaveData()
        {

        }
        ///<summary>
        ///ローカルにあるサーバーアドレス情報読み込み
        ///</summary>
        ///<returns>true:読み込み成功, false:読み込み失敗</returns>
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
