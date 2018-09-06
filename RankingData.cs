using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpRanking
{
    class RankingData
    {
        public static UInt64 GameID { private set; get; }
        public UInt64 DataID { private set; get; }
        public DateTime SaveTime { private set; get; }

        public string DataName { private set; get; }
        public Score Score { private set; get; }

        public static bool SetGameID(UInt64 id)
        {
            if (id >= 100) return false;
            GameID = id;
            return true;
        }



    }
}
