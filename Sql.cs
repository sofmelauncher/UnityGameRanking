using System;
using System.Data.SQLite;

namespace SQLite
{
    class SQLite
    {
        
        private static SQLiteConnection _conn = null;
        private string DbFile = ConfigPath.LocalUserAppDataPath + "\\ranking.db";

        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS Ranking("
            + "DataID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
            + "SaveTime DATETIME NOT NULL,"
            + "DataName VARCHAR(100) NULL,"
            + "ScoreValue CHAR NOT NULL)";
        private const string InsertCommand = "INSERT INTO Ranking (SaveTime, DataName, ScoreValue) VALUES (@1, @2, @3)";
        private const string SelectCommand = "SELECT* FROM Ranking";

        /// <summary>
        /// データベースに接続
        /// </summary>
        public void ConnectionOpen()
        {
            _conn = new SQLiteConnection();
            _conn.ConnectionString = "Data Source=" + DbFile + ";Version=3;";
            _conn.Open();
        }


        /// <summary>
        /// テーブルの作成
        /// </summary>
        public void CreateTable()
        {
            SQLiteCommand command = _conn.CreateCommand();
            command.CommandText = CreateTableCommand;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// レコードを挿入
        /// </summary>
        public void InsertRecord()
        {
            for (int i = 0; i < 10; i++)
            {
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = InsertCommand;
                SQLiteParameter parameter1 = command.CreateParameter();
                parameter1.ParameterName = "@1";
                parameter1.Value = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                SQLiteParameter parameter2 = command.CreateParameter();
                parameter2.ParameterName = "@2";
                parameter2.Value = i.ToString() + "er";
                SQLiteParameter parameter3 = command.CreateParameter();
                parameter3.ParameterName = "@3";
                parameter3.Value = (i + 1).ToString();
                command.Parameters.Add(parameter1);
                command.Parameters.Add(parameter2);
                command.Parameters.Add(parameter3);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// レコードを取得
        /// </summary>
        public void SelectRecord()
        {
            // 全データの取得
            SQLiteCommand command = _conn.CreateCommand();
            command.CommandText = SelectCommand;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(string.Format("ID = {0,4}, TIME = {1}, Name = {2,10}, Score = {3,5:#.###}",
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3)
                ));
            }
        }


        /// <summary>
        /// データベース接続を閉じる
        /// </summary>
        public void ConnectionClose()
        {
            _conn.Close();
        }
    }
}

