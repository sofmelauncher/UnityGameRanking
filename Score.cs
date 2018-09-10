using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranking
{
    public class Score
    { 
        public static ScoreType Type { private set; get; }

        public string ScoreValue { private set; get; }

        public static void SetScoreType(ScoreType type)
        {
            Type = type;
            return;
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

        public void Save(string data)
        {
            this.ScoreValue = data;
        }


    }
}
