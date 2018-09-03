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
            var www = new WWWRunking();
            www.GetData();
        }
    }

    class WWWRunking
    {
        public void GetData()
        {
            string url = "http://localhost/runking/GetData.php";

            //文字コードを指定する
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("UTF-8");

            //POST送信する文字列を作成
            string postData =
                "id=1&word=" +
                System.Web.HttpUtility.UrlEncode("keyword", enc);

            System.Net.WebClient wc = new System.Net.WebClient();
            //文字コードを指定する
            wc.Encoding = enc;
            //ヘッダにContent-Typeを加える
            wc.Headers.Add("Content-Type", "application/plain");
            //データを送信し、また受信する
            string resText = wc.UploadString(url, postData);
            Console.WriteLine(postData);
            wc.Dispose();

            //受信したデータを表示する
            Console.WriteLine("data：");
            Console.WriteLine(resText);
        }
    }
}
