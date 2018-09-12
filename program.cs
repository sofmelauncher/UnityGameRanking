using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Ranking.RankingManager m = new Ranking.RankingManager("めじぇど", 2, Ranking.OrderType.DESC);
            m.Init();
            m.SaveData(1, "test1");
            Console.WriteLine(m.Version);
            Console.WriteLine(m.GetLogPath);
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            var data = m.GetData();
            foreach (var e in data)
            {
                Console.WriteLine(e.ScoreValue);
                Console.WriteLine(e.DataName);
            }
        }
    }
}
