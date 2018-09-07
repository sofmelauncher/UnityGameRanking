using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpRanking
{
    public class RankingData
    {
        public static UInt64 GameID { private set; get; }

        public UInt64 DataID { private set; get; }
        public DateTime SaveTime { private set; get; }

        public string DataName { private set; get; }
        //private Score Score { set; get; }

        public static void SetScoreType(ScoreType type)
        {
            Type = type;
            return;
        }
        public static ScoreType Type { private set; get; }
        public string ScoreValue { private set; get; }

        public RankingData(string data, string name = "")
        {
            this.ScoreValue = data;
            this.DataName = name;
        }

        public static bool SetGameID(UInt64 id)
        {
            if (id >= 100) return false;
            GameID = id;
            return true;
        }

        public double ToDouble()
        {
            return double.Parse(this.ScoreValue);
        }

        public TimeSpan ToTime()
        {
            long t = long.Parse(ScoreValue);
            TimeSpan ts = new TimeSpan(t);
            return ts;
        }

        public Dictionary<string, string> Dictionary()
        {
            return new Dictionary<string, string>
                {
                    { "GameID", RankingData.GameID.ToString() },
                    { "DataID", "0" },
                    { "DataName", this.DataName },
                    { "Score", this.ScoreValue }
                };
        }

    }
}
