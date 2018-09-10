using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ranking
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = new RankingManager("uni",　1, ScoreType.NUMBER, OrderType.ASC);
            w.Init();

            //List<RankingData> aa = w.GetData();
            //foreach (var s in aa)
            //{
            //    Log.Debug(string.Format("ID = {0,4}, TIME = {1,20}, Name = {2,10}, Score = {3,5:#.###}",
            //        s.DataID,
            //        s.SaveTime,
            //        s.DataName,
            //        s.ScoreValue
            //        ));
            //}
            w.SaveData(1.4949, "jdwoj");

        }
    }
}
