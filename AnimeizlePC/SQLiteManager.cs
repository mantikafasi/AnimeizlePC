using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Diagnostics;
using System.Json;
using Newtonsoft.Json;
namespace AnimeizlePC
{
    //DATABASE E JSONSERIELZER İLE KOY AYNI ŞEKİLDE AL
    class SQLiteManager
    {
        public enum dataType
        {
            lastwatched,downloadedAnime
        }
        SQLiteConnection con;
        private static SQLiteManager instance;
        public static SQLiteManager getInstance() {
            if (instance == null) {
                instance = new SQLiteManager();
            }
            return instance;
        }
        SQLiteManager()
        {
            con = new SQLiteConnection("Data Source=database.sqlite");
            con.Open();
            new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS lastwatched (ID INTEGER PRIMARY KEY AUTOINCREMENT,content VARCHAR(45),episodeurl VARCHAR(45),unique(content,episodeurl))", con).ExecuteNonQuery();
            new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS downloadedAnime (ID INTEGER PRIMARY KEY AUTOINCREMENT,content VARCHAR(45),episodeurl VARCHAR(45),unique(content,episodeurl))", con).ExecuteNonQuery();
           }
        public void AddData(Anime anime,dataType datatype=0)
        {
            anime.episodes = null;
            
            //string data = Enum.GetName(typeof(dataType), datatype);
            string data = datatype.ToString();
            SQLiteCommand command = new SQLiteCommand(string.Format("INSERT INTO {0}(content,episodeurl) VALUES(@content,@episodeurl)",data), con);
            command.Parameters.AddWithValue("content", anime.ToString());
            command.Parameters.AddWithValue("episodeurl", anime.episode.episodeurl);
            try
            {
                command.ExecuteNonQuery();
            } catch (Exception) { }
        }
        public List<Anime> GetData(dataType type = 0)
        {
            List<Anime> list = new List<Anime>();
            string data = Enum.GetName(typeof(dataType), type);
            SQLiteDataReader reader = new SQLiteCommand(string.Format("SELECT * FROM {0}", data), con).ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Anime(reader));
            }
            return list;
        }
        public void DeleteData(Anime episode, dataType datatype = 0) { 
            string data = Enum.GetName(typeof(dataType), datatype);
            new SQLiteCommand(string.Format("DELETE FROM {0} where episodeurl={1}",data, episode.episode.episodeurl), con).ExecuteNonQuery();
        }

        public bool UpdateData(Anime anime,dataType type = 0)
        {
             SQLiteCommand command= new SQLiteCommand(string.Format("UPDATE {0} SET content=@content WHERE episodeurl=@episodeurl",type.ToString()), con);
             command.Parameters.AddWithValue("content", anime.ToString());
             command.Parameters.AddWithValue("episodeurl", anime.episode.episodeurl);
             Trace.WriteLine(command.CommandText + command.Parameters.ToString());
             int rows = command.ExecuteNonQuery();
             return rows > 0;
        }
    }
}
