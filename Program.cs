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
            //var w = new RankingManager(1, ScoreType.Number, OrderType.ASC);
            //if (w.Init())
            //{
            //    w.DataSetAndLoad(1.3, "aa");
            //}
            //else
            //{
            //    Console.WriteLine("Filed to init.");
            //}

            SQLite.SQLite s = new SQLite.SQLite();
            s.ConnectionOpen();
            s.CreateTable();
            //s.InsertRecord();
            s.SelectRecord();
            s.ConnectionClose();

        }
    }
}
