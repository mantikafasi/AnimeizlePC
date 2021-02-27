using EmergenceGuardian.MpvPlayerUI;
using Mpv.NET.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnimeizlePC
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary> 
    public partial class VideoPlayer : Window
    {
		Anime anime; 
		public MpvPlayer player
		{
			get { return Player.Host.Player; }
			set { Player.Host.Player = value; }
		}

        private SQLiteManager manager;
        public VideoPlayer(Anime anim)
        {
            InitializeComponent();
			anime = anim;
			manager=SQLiteManager.getInstance();

			Player.MediaPlayerInitialized += (obj, e) =>
            {
                string watchurl;
                watchurl = !string.IsNullOrEmpty(anime.episode.downloadLocation) ? anime.episode.downloadLocation : anime.episode.watchurl;
				  
                 player.EnableYouTubeDl();
				 //TODO CONFİG DOSYASINI AYARLAR
				 if (watchurl.Contains("sibnet")) { player.API.SetPropertyString("ytdl-raw-options", "config-location=\".\\lib\\sibnet.conf\""); }
				 



				 player.Load(watchurl);
				 player.MediaError += onError;
				 player.MediaLoaded += (o, s) =>
				 {
					 float progress = anime.episode.progress;
                     
					 if (progress != 0.0f)
					 {
                         Dispatcher.Invoke(() =>
                         {
                            player.Position = TimeSpan.FromMilliseconds(progress * player.Duration.TotalMilliseconds/100);
                         });
						 
					 }


				 };
				 player.Resume();
			 };


        }

		private void WindowOnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
			if (player.Position.Milliseconds != 0 && player.Duration.Milliseconds != 0)
            {
                anime.episode.progress = (float) (player.Position.TotalMilliseconds / player.Duration.TotalMilliseconds)*100;
                manager.UpdateData(anime);
                Player.Host.Player.Dispose();
            }

		}
		public void onError(object sender,EventArgs e)
        {
			Dispatcher.Invoke(() => {Close(); });
            MessageBox.Show("Video Yüklenirken bir hata oluştu Alternatif Desteklenmiyor veya Video Silinmiş Olabilir.Site Tarayıcıdan Açılıyor.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            Process.Start(anime.episode.watchurl);
		}

	}
}
