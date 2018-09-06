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
            var w = new RankingManager();
            w.PostString = "aaai333";
            if (w.Init(1, ScoreType.Number, OrderType.ASC))
            {
                w.GetData();
            }
            else
            {
                Console.WriteLine("filed init.");
            }
        }    
    }
}
