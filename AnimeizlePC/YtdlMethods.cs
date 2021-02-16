
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace AnimeizlePC
{
    class YtdlMethods
    {
        static YtdlMethods instance;
        SQLiteManager manager; 

        public static YtdlMethods getInstance()
        {
            if (instance == null) {
                instance = new YtdlMethods();
            }
            return instance;
        }
        public YtdlMethods() {
            manager = SQLiteManager.getInstance();
        }
        
        public string getStreamURL(string url) {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            string arguments="";
            if (url.Contains("sibnet")) {
                arguments += "--user-agent \"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36\" --referer \"https://video.sibnet.ru/\" -i "; }
            startInfo.Arguments = "/C .\\Resources\\youtube-dl.exe -g " + arguments +url;
            process.StartInfo = startInfo;
            process.Start();
            string output=null;
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
        string directory= System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public void openWithmpv(Anime anime)
        {
            VideoPlayer player = new VideoPlayer(anime);
            player.Show();
        }
        public void openWithmpvOld(Anime anime) {
            string url = anime.episode.watchurl.Replace("[","").Replace("]","");
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";

            startInfo.WorkingDirectory = directory;

            string arguments = "";

            if (url.Contains("sibnet"))
            {
                arguments += "--ytdl-raw-options=config-location=\".\\lib\\sibnet.conf\" ";
               
            }
            startInfo.Arguments = "/C echo Yükleniyor Lütfen Bekleyin && .\\lib\\mpv.exe " + arguments + url ;
            process.StartInfo = startInfo;
            Trace.WriteLine(startInfo.Arguments);
            process.Start();
        }

        Queue<Anime> queue = new Queue<Anime>();
        BackgroundWorker worker = new BackgroundWorker();
        public void addToQueue(Anime anime) {
            queue.Enqueue(anime);
            
            if (!worker.IsBusy) { 
                worker.WorkerReportsProgress = false; //şu anda sadece cmd ekranı gözükecek
                worker.DoWork += download;
                // worker.RunWorkerCompleted += indirmebitti; ileride indirmeler bitince bildirim göster
                worker.RunWorkerAsync();
            }

        }

        public void download(object sender,DoWorkEventArgs e) {
            while (queue.Count > 0) {
                Anime anime = queue.Dequeue();
                AnimeEpisode episode = anime.episode;
                episode.watchurl=episode.watchurl.Replace("[", "").Replace("]", "");
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                string arguments = "";
                string saveLocation = "";

                if (!string.IsNullOrEmpty(App.settings.saveLocation))
                {
                    saveLocation += App.settings.saveLocation+"\\";
                    
                    string videoname = App.settings.VideoName;
                    if (!string.IsNullOrEmpty(videoname))
                    {
                        videoname = videoname.Replace("%bolumadi", episode.episodename);
                        saveLocation += videoname +".mp4";
                    }
                    arguments += "-o \"" +saveLocation +"\" ";
                    anime.episode.downloadLocation = saveLocation;
                }

                if (episode.watchurl.Contains("sibnet"))
                {
                    arguments += @"--config-location .\lib\sibnet.conf ";
                }

                startInfo.WorkingDirectory = directory;
                startInfo.Arguments = @"/C .\lib\youtube-dl.exe " + arguments  +episode.watchurl;
                Trace.WriteLine(startInfo.Arguments);
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                
                if (File.Exists(saveLocation))
                {
                    manager.AddData(anime, SQLiteManager.dataType.downloadedAnime);
                    
                }
                else
                {
                    MessageBox.Show("Bölüm İndirilirken Bir Hata oluştu.");
                }

                
            }
        }


    }
}
