using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ranking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ranking.Tests
{
    [TestClass()]
    public class RankingManagerTests
    {
        [TestMethod()]
        public void RankingManagerTest()
        {
            RankingManager.Inst.Setting("神のゲーム", 1, Ranking.OrderType.DESC, false);

            RankingManager.Inst.SaveData(10.5, "ジェイソン");

            //データ取得
            //例
            var data = RankingManager.Inst.GetData();
            foreach (var e in data)
            {
                Console.WriteLine(e.ScoreValue);
                Console.WriteLine(e.DataName);
            }
        }
    }
}