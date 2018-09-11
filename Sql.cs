using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace Ranking
{
    namespace SQLite
    {
        class SQLite
        {
            private static UInt64 GameID { set; get; }
            private static SQLiteConnection _conn = null;
            private static UInt64 limit = 5;
            private string DbFilePath = ConfigPath.LocalUserAppDataPath;
            private const string FileNmae = "ranking.db";

            private const string CreateTableCommand =
                "CREATE TABLE IF NOT EXISTS @GameName (DataID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
                + "SaveTime DATETIME NOT NULL,"
                + "DataName VARCHAR(100) NULL,"
                + "ScoreValue DOUBLE NOT NULL)";

            private const string InsertCommand =
                "INSERT INTO @GameName (SaveTime, DataName, ScoreValue) VALUES (@1, @2, @3)";

            private const string SelectCommand =
                "SELECT * FROM @GameName ORDER BY ScoreValue {0}, SaveTime DESC LIMIT {1};";

            private const string AllSelectCommand =
                "SELECT * FROM @GameName;";

            public static void SetLimit(UInt64 l)
            {
                SQLite.limit = l;
            }

            /// <summary>
            /// データベースに接続
            /// </summary>
            public void ConnectionOpen()
            {
                _conn = new SQLiteConnection();
                _conn.ConnectionString =
                    "Data Source=" + DbFilePath + "\\" + FileNmae + ";Version=3;";
                try
                {
                    _conn.Open();
                }
                catch (InvalidOperationException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.Configuration.ConfigurationException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                Ranking.Log.Info("【Success】【SQL】Connect to [Data Source = " + DbFilePath + "\\" + FileNmae + "].");
                return;
            }


            /// <summary>
            /// テーブルの作成
            /// </summary>
            public void CreateTable()
            {
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = GameIDINCommand(CreateTableCommand);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (InvalidCastException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.IO.IOException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                Ranking.Log.Info("【Success】【SQL】Created of [" + TableName + "] table.");
                return;
            }

            /// <summary>
            /// レコードを挿入
            /// </summary>
            public void InsertRecord(Ranking.RankingData data)
            {
                Ranking.Log.Info("【SQL】Ranking Data [" + data.ToString() + "].");
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = GameIDINCommand(InsertCommand);

                SQLiteParameter SaveTime = command.CreateParameter();
                SaveTime.ParameterName = "@1";
                SaveTime.Value = data.SaveTime;

                SQLiteParameter DataName = command.CreateParameter();
                DataName.ParameterName = "@2";
                DataName.Value = data.DataName;

                SQLiteParameter ScoreValue = command.CreateParameter();
                ScoreValue.ParameterName = "@3";
                ScoreValue.Value = data.ToDouble;

                command.Parameters.Add(SaveTime);
                command.Parameters.Add(DataName);
                command.Parameters.Add(ScoreValue);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (InvalidCastException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.IO.IOException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }

                Ranking.Log.Info("【Success】【SQL】Execute [" + command.CommandText + "].");
                Ranking.Log.Info("【SQL】@1 is [" + command.Parameters["@1"].Value.ToString() + "].");
                Ranking.Log.Info("【SQL】@2 is [" + command.Parameters["@2"].Value.ToString() + "].");
                Ranking.Log.Info("【SQL】@3 is [" + command.Parameters["@3"].Value.ToString() + "].");
                return;
            }

            /// <summary>
            /// 指定の個数、並びのレコードを取得
            /// </summary>
            /// <param name="type">OrderType型</param>
            /// <returns>RankignData型のレコードのリスト</returns>
            public List<Ranking.RankingData> SelectRecord(Ranking.OrderType type)
            {
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = string.Format(GameIDINCommand(SelectCommand), type.ToString(), SQLite.limit);
                SQLiteDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (InvalidCastException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.IO.IOException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                Ranking.Log.Info("【Success】【SQL】Executed [" + command.CommandText + "].");


                var list = new List<Ranking.RankingData>();

                while (reader.Read())
                {
                    list.Add(new Ranking.RankingData(Convert.ToUInt64(reader.GetInt32(0)), reader.GetDateTime(1), reader.GetString(2), reader.GetDouble(3)));
                }


                return list;
            }

            /// <summary>
            /// 全レコードの取得
            /// </summary>
            /// <returns>全レコードのRankingDataクラス型のリスト</returns>
            public List<Ranking.RankingData> AllSelectRecord()
            {
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = GameIDINCommand(AllSelectCommand);
                SQLiteDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (InvalidCastException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (System.IO.IOException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
                Ranking.Log.Info("【Success】【SQL】Exexuted [" + command.CommandText + "].");

                var list = new List<Ranking.RankingData>();

                while (reader.Read())
                {
                    list.Add(new Ranking.RankingData(Convert.ToUInt64(reader.GetInt32(0)), reader.GetDateTime(1), reader.GetString(2), reader.GetDouble(3)));
                }

                return list;
            }


            /// <summary>
            /// データベース接続を閉じる
            /// </summary>
            public void ConnectionClose()
            {
                _conn.Close();
                Ranking.Log.Info("【Success】【SQL】Data base close.");
                return;
            }

            /// <summary>
            /// ゲーム名設定
            /// </summary>
            public static void SetGameName(UInt64 id)
            {
                SQLite.GameID = id;
                Log.Info("【Success】【SQL】Set GameID is " + SQLite.GameID.ToString());
            }

            private void ConsoleWriteData(SQLiteDataReader data)
            {
                try
                {
                    while (data.Read())
                    {
                        Console.WriteLine(string.Format("ID = {0,4}, TIME = {1,20}, Name = {2,10}, Score = {3,5:#.###}",
                            data.GetInt32(0),
                            data.GetString(1),
                            data.GetString(2),
                            data.GetDouble(3)
                        ));
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    Ranking.Log.Fatal(ex.Message);
                }
            }

            /// <summary>
            /// SQLコマンドにゲーム名テーブルを指定する
            /// </summary>
            /// <param name="command"></param>
            /// <returns></returns>
            private string GameIDINCommand(string command)
            {
                string result = Regex.Replace(command, "@GameName", TableName);
                return result;
            }

            private string TableName {
                get {
                    return "GameID" + SQLite.GameID.ToString();
                }
            }
        }
    }

}