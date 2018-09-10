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
            var w = new RankingManager("uni",　1, ScoreType.NUMBER, OrderType.ASC, false);
            w.Init();
            w.GetData();

        }
    }
}
