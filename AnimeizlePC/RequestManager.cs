using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Net.Http.Json;
using System.Json;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace AnimeizlePC
{
    class RequestManager
    {
        public static RequestManager _instance;
        public static RequestManager getInstance() {
            if (_instance == null)
            {
                _instance = new RequestManager();
            }
            return _instance;
        }
        HttpClient _client;
        public RequestManager(){
            _client = new HttpClient();

        }

        public static string _apiurl= "https://mantikralligi.pythonanywhere.com";
        
        public List<Anime> Search(string searchid)
        {
            string url = _apiurl;
            if (!string.IsNullOrEmpty(searchid)) {
                url += "/search?q=" + searchid;
            }

            List < Anime > list = new List<Anime>();
            
            List<Dictionary<string,string>> array = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>((_client.GetAsync(url).Result.Content.ReadAsStringAsync().Result.Replace("None","\"\"")));

            foreach (var item in array)
            {
                list.Add(new Anime { AnimeName = item["animename"], EnglishName = item["englishname"], ImageURL = item["imageurl"], MyAnimeListURL = item["myanimelisturl"], Ozet = item["ozet"], turkanimeurl = item["turkanimeurl"] });
            }

            return list;
        }


        public Anime getDetails(Anime anime) {
            string url = _apiurl + "/anime?animeid=" + anime.turkanimeurl;


            
            List<Dictionary<string, object>> array = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>((_client.GetAsync(url).Result.Content.ReadAsStringAsync().Result));
            List<AnimeEpisode> episodes = new List<AnimeEpisode>();
            foreach (var item in array)
            {
                episodes.Add(new AnimeEpisode { ID = (long)item["ID"], epindex = (long)item["epindex"], episodename = (string)item["episodename"], episodeurl = (string)item["episodeurl"], watchurl = (string)item["watchurls"] });


            }
            anime.episodes = episodes;

            return anime;
            
        }
    }

    public class Anime
    {
        public Anime(SQLiteDataReader reader)
        {
            Anime a = JsonConvert.DeserializeObject<Anime>(reader.GetString(reader.GetOrdinal("content")));
            AnimeName = a.AnimeName;
            ImageURL = a.ImageURL;
            EnglishName = a.EnglishName;
            turkanimeurl = a.turkanimeurl;
            MyAnimeListURL = a.MyAnimeListURL;
            Ozet = a.Ozet;
            episode = a.episode;
            episodes = a.episodes;
        }
        public Anime() { }
        
        public string AnimeName { get; set; }
        public string ImageURL { get; set; }
        public string EnglishName { get; set; }
        public string turkanimeurl { get; set; }
        public string MyAnimeListURL { get; set; }
        public string Ozet { get; set; }
        public AnimeEpisode episode { get; set; }

        public Anime Clone => (Anime) MemberwiseClone();

        public List<AnimeEpisode> episodes { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class AnimeEpisode
    {
        public long ID { get; set; }
        public long epindex { get; set; }
        public string episodename { get; set; }
        public string episodeurl { get; set; }
        public string watchurl { get; set; }
        public float progress { get; set; }
        public string downloadLocation { get; set; }  
    }

}
