using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CsharpRanking
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = new RankingManager("uni",　1, ScoreType.Number, OrderType.ASC);
            if (w.Init())
            {
                w.DataSetAndLoad(1.8, "aa");
            }
            else
            {
                Console.WriteLine("Filed to init.");
            }
            w.GetData();

        }
    }
}
