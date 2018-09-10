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
            var w = new RankingManager("uni",　1, ScoreType.NUMBER, OrderType.DESC,false);
            if (w.Init())
            {
                //w.DataSetAndLoad(1.8, "aa");
            }
            else
            {
                Log.Debug("Filed to init.");
            }
            w.GetData();

        }
    }
}
