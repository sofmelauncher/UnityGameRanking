﻿using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace SQLite
{
    class SQLite
    {
        private static UInt64 GameID { set; get; }
        private static SQLiteConnection _conn = null;
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
            "SELECT * FROM @GameName ORDER BY SaveTime DESC;";

        private const string AllSelectCommand = 
            "SELECT * FROM @GameName;";

        /// <summary>
        /// データベースに接続
        /// </summary>
        public void ConnectionOpen()
        {
            _conn = new SQLiteConnection();
            _conn.ConnectionString = 
                "Data Source=" + DbFilePath + "\\" + FileNmae + ";Version=3;";
            _conn.Open();
            return;
        }


        /// <summary>
        /// テーブルの作成
        /// </summary>
        public void CreateTable()
        {
            SQLiteCommand command = _conn.CreateCommand();
            command.CommandText = GameIDINCommand(CreateTableCommand, SQLite.GameID);
            command.ExecuteNonQuery();
            return;
        }

        /// <summary>
        /// レコードを挿入
        /// </summary>
        public void InsertRecord(CsharpRanking.RankingData data)
        {
            
            SQLiteCommand command = _conn.CreateCommand();
            command.CommandText = GameIDINCommand(InsertCommand, SQLite.GameID);

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
            command.ExecuteNonQuery();
            return;
        }

        /// <summary>
        /// レコードを取得
        /// </summary>
        public void SelectRecord()
        {
            SQLiteCommand command = _conn.CreateCommand();
            command.CommandText = GameIDINCommand(SelectCommand, SQLite.GameID);
            var reader = command.ExecuteReader();
            this.ConsoleWriteData(reader);
            return;
        }

        public void AllSelectRecord()
        {
            // 全データの取得
            SQLiteCommand command = _conn.CreateCommand();
            command.CommandText = GameIDINCommand(AllSelectCommand, SQLite.GameID);
            var reader = command.ExecuteReader();
            this.ConsoleWriteData(reader);
            return;
        }


        /// <summary>
        /// データベース接続を閉じる
        /// </summary>
        public void ConnectionClose()
        {
            _conn.Close();
            return;
        }

        /// <summary>
        /// ゲーム名設定
        /// </summary>
        public static void SetGameName(UInt64 id)
        {
            SQLite.GameID = id;
        }

        private void ConsoleWriteData(SQLiteDataReader data)
        {
            while (data.Read())
            {
                Console.WriteLine(string.Format("ID = {0,4}, TIME = {1,28}, Name = {2,10}, Score = {3,5:#.###}",
                    data.GetInt32(0),
                    data.GetString(1),
                    data.GetString(2),
                    data.GetDouble(3)
                ));
            }
        }

        /// <summary>
        /// SQLコマンドにゲーム名テーブルを指定する
        /// </summary>
        /// <param name="command"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GameIDINCommand(string command, UInt64 id)
        {
            Console.WriteLine(command);
            string result = Regex.Replace(command, "@GameName", id.ToString());
#if DEBUG
            Console.WriteLine(result);
#endif
            return result;
        }
    }
}

