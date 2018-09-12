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
            private string DbFilePath = Path.LocalPath;
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

            private const string DiffCreateTableCommand =
                "CREATE TABLE IF NOT EXISTS @GameNamediff (DataID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
                + "SaveTime DATETIME NOT NULL,"
                + "DataName VARCHAR(100) NULL,"
                + "ScoreValue DOUBLE NOT NULL)";

            private const string DiffInsertCommand =
                 "INSERT INTO @GameNamediff (SaveTime, DataName, ScoreValue) VALUES (@1, @2, @3);";

            private const string DiffAllSelectCommand =
                "SELECT * FROM @GameNamediff;";

            private const string DiffCheckCommand = "SELECT * FROM @GameNamediff LIMIT 1;";

            private const string DiffAllDeleteCommand = "DELETE FROM @GameNamediff;";

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
                Log.Info("【SQL】Execute [" + _conn.ConnectionString + "]");
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
                SQLiteCommand command_1 = _conn.CreateCommand();
                command_1.CommandText = GameIDINCommand(CreateTableCommand);

                SQLiteCommand command_2 = _conn.CreateCommand();
                command_2.CommandText = GameIDINCommand(DiffCreateTableCommand);

                Log.Info("【SQL】Execute [" + command_1.CommandText + "]");
                Log.Info("【SQL】Execute [" + command_2.CommandText + "]");
                try
                {
                    command_1.ExecuteNonQuery();
                    command_2.ExecuteNonQuery();
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
                Ranking.Log.Info("【Success】【SQL】Created of [" + TableName + "diff] table.");

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

                Ranking.Log.Info("【SQL】Execute [" + command.CommandText + "].");
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
                Ranking.Log.Info("【Success】【SQL】Executed [" + command.CommandText + "].");
                Ranking.Log.Info("【SQL】@1 is [" + command.Parameters["@1"].Value.ToString() + "].");
                Ranking.Log.Info("【SQL】@2 is [" + command.Parameters["@2"].Value.ToString() + "].");
                Ranking.Log.Info("【SQL】@3 is [" + command.Parameters["@3"].Value.ToString() + "].");
                return;
            }

            /// <summary>
            /// 差分データベースにデータ挿入
            /// </summary>
            /// <param name="data"></param>
            public void DiffInsertRecord(Ranking.RankingData data)
            {
                Ranking.Log.Info("【SQL】【Diff】Ranking Data [" + data.ToString() + "].");
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = GameIDINCommand(DiffInsertCommand);

                SQLiteParameter SaveTime = command.CreateParameter();
                SaveTime.ParameterName = "@1";
                SaveTime.Value = data.SaveTime.ToString("yyyy - MM - dd HH: mm:ss");

                SQLiteParameter DataName = command.CreateParameter();
                DataName.ParameterName = "@2";
                DataName.Value = data.DataName;

                SQLiteParameter ScoreValue = command.CreateParameter();
                ScoreValue.ParameterName = "@3";
                ScoreValue.Value = data.ToDouble;

                command.Parameters.Add(SaveTime);
                command.Parameters.Add(DataName);
                command.Parameters.Add(ScoreValue);

                Ranking.Log.Info("【SQL】【Diff】Execute [" + command.CommandText + "].");
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
                Ranking.Log.Info("【Success】【SQL】【Diff】Execute [" + command.CommandText + "].");
                Ranking.Log.Info("【SQL】【Diff】@1 is [" + command.Parameters["@1"].Value.ToString() + "].");
                Ranking.Log.Info("【SQL】【Diff】@2 is [" + command.Parameters["@2"].Value.ToString() + "].");
                Ranking.Log.Info("【SQL】【Diff】@3 is [" + command.Parameters["@3"].Value.ToString() + "].");
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
                Ranking.Log.Info("【SQL】Execute [" + command.CommandText + "].");
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
                Ranking.Log.Info("【SQL】Exexuted [" + command.CommandText + "].");
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

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new Ranking.RankingData(Convert.ToUInt64(reader.GetInt32(0)), reader.GetDateTime(1), reader.GetString(2), reader.GetDouble(3)));
                    }
                }

                return list;
            }

            /// <summary>
            /// 差分用全データ取得
            /// </summary>
            /// <returns></returns>
            public List<Ranking.RankingData> DiffAllSelectRecord()
            {
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = GameIDINCommand(DiffAllSelectCommand);
                Ranking.Log.Info("【SQL】【Diff】Exexute [" + command.CommandText + "].");
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
                Ranking.Log.Info("【Success】【SQL】【Diff】Exexuted [" + command.CommandText + "].");

                var list = new List<Ranking.RankingData>();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        list.Add(new Ranking.RankingData(Convert.ToUInt64(reader.GetInt32(0)), reader.GetDateTime(1), reader.GetString(2), reader.GetDouble(3)));
                    }
                }

                return list;
            }
            
            /// <summary>
            /// Diffデータベースにデータが存在するかどうか
            /// </summary>
            /// <returns>true:存在する, false:存在しない</returns>
            public bool IsDiffDBIseet()
            {
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = GameIDINCommand(DiffCheckCommand);
                Ranking.Log.Info("【SQL】【Diff】Exexute [" + command.CommandText + "].");
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
                Ranking.Log.Info("【Success】【SQL】【Diff】Exexuted [" + command.CommandText + "].");

                if (reader != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void DiffAllDelete()
            {
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = GameIDINCommand(DiffAllDeleteCommand);

                Ranking.Log.Info("【SQL】【Diff】Execute [" + command.CommandText + "].");
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
                Ranking.Log.Info("【Success】【SQL】【Diff】Execute [" + command.CommandText + "].");
                return;
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

            /// <summary>
            /// SQLコマンドにゲームdiff名テーブルを指定する
            /// </summary>
            /// <param name="command"></param>
            /// <returns></returns>
            private string DiffGameIDINCommand(string command)
            {
                string result = Regex.Replace(command, "@GameName", TableName + "diff");
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