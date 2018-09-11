using System.Collections.Generic;


namespace Ranking
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = new RankingManager("uni",　1, OrderType.DESC);
            w.Init();
            w.SaveData(2, "すー");

            List<RankingData> aa = w.GetData();
            foreach (var s in aa)
            {
                Log.Debug(s.ToString());
            }

        }
    }
}
