using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ranking
{
    /// <summary>
    /// ランキングデータクラス
    /// </summary>
    public class RankingData:IEquatable<RankingData>
    {
        public static UInt64 GameID { private set; get; }
        public UInt64 DataID { private set; get; }
        public DateTime SaveTime { private set; get; }

        public string DataName { private set; get; }

        public double ScoreValue { private set; get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">スコアデータ</param>
        /// <param name="name">データ名</param>
        public RankingData(double data, string name = "")
        {
            this.ScoreValue = data;
            this.SaveTime = DateTime.Now;
            this.DataName = name;
        }

        public RankingData(UInt64 dataid, DateTime time, string name, double data)
        {
            this.DataID = dataid;
            this.ScoreValue = data;
            this.SaveTime = time;
            this.DataName = name;
        }
        [JsonConstructor]
        public RankingData(string dataid, string savetime, string dataname, string scorevalue)
        {
            this.DataID = UInt64.Parse(dataid);
            this.SaveTime = DateTime.Parse(savetime);
            this.ScoreValue = Double.Parse(scorevalue);
            this.DataName = dataname;
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
                return this.ScoreValue;
            }
        }

        /// <summary>
        /// スコアデータをTimeSpan型で取得
        /// </summary>
        public TimeSpan ToTime
        {
            get {
                long t = long.Parse(ScoreValue.ToString());
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
                    { "ScoreValue", this.ScoreValue.ToString() },
                    { "SaveTime", this.SaveTime.ToString("yyyy-MM-dd HH:mm:ss") }
                };
        }
        public override int GetHashCode()
        {
            return GameID.GetHashCode() ^
                    DataID.GetHashCode() ^
                    SaveTime.GetHashCode() ^
                    DataName.GetHashCode() ^
                    ScoreValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as RankingData;
            if (other == null) return false;
            return this.DataID == other.DataID &&
                    this.DataName == other.DataName &&
                    this.SaveTime == other.SaveTime &&
                    this.ScoreValue == other.ScoreValue;
        }
        public bool Equals(RankingData other)
        {
            if (other == null) return false;
            return (this.DataID.Equals(other.DataID));
        }

        public override string ToString()
        {
            return string.Format("GameID = {0,2}, Time = {1,10}, DataID = {2,2}, Name = {3}, Score = {4,5:#.###}",
                            RankingData.GameID.ToString(),
                            this.SaveTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            this.DataID.ToString(),
                            this.DataName,
                            this.ScoreValue
                        );
        }

    }
}
