using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpRanking
{
    /// <summary>
    /// ランキングデータクラス
    /// </summary>
    public class RankingData
    {
        public static UInt64 GameID { private set; get; }
        public UInt64 DataID { private set; get; }
        public DateTime SaveTime { private set; get; }
        public string DataName { private set; get; }

        private static ScoreType Type { set; get; }
        private string ScoreValue { set; get; }

        /// <summary>
        /// ランキングデータのスコアのデータ型を指定
        /// </summary>
        /// <param name="type">ScoreType型, スコアのデータ型</param>
        public static void SetScoreType(ScoreType type)
        {
            Type = type;
            return;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">スコアデータ</param>
        /// <param name="name">データ名</param>
        public RankingData(string data, string name = "")
        {
            this.ScoreValue = data;
            this.DataName = name;
            this.SaveTime = DateTime.Now;
        }

        /// <summary>
        /// statc, ゲームIDセット
        /// </summary>
        /// <param name="id">ゲームID</param>
        /// <returns>true:成功, false:失敗</returns>
        public static bool SetGameID(UInt64 id)
        {
            if (id == 0) return false;
            RankingData.GameID = id;
            return true;
        }

        /// <summary>
        /// スコアデータをdouble型で取得
        /// </summary>
        public double ToDouble
        {
            get{
                return double.Parse(this.ScoreValue);
            }
        }

        /// <summary>
        /// スコアデータをTimeSpan型で取得
        /// </summary>
        public TimeSpan ToTime
        {
            get {
                long t = long.Parse(ScoreValue);
                TimeSpan ts = new TimeSpan(t);
                return ts;
            }
        }

        /// <summary>
        /// POST用のランキングデータを辞書化して取得する
        /// </summary>
        /// <returns>連想配列型(辞書)のスコアデータ</returns>
        public Dictionary<string, string> Dictionary()
        {
            return new Dictionary<string, string>
                {
                    { "GameID", RankingData.GameID.ToString() },
                    { "DataID", "0" },
                    { "DataName", this.DataName },
                    { "Score", this.ScoreValue },
                    { "Time", this.SaveTime.ToString("yyyy-MM-dd HH:mm:sss") }
                };
        }

    }
}
