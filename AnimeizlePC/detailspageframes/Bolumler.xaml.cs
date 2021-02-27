using System;
using System.Collections.Generic;
using System.Globalization;
using System.Json;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace AnimeizlePC.detailspageframes
{
    /// <summary>
    /// Interaction logic for Bolumler.xaml
    /// </summary>
    public partial class Bolumler : Page
    {
        private Anime anime; 
        SQLiteManager sqlitemanager = SQLiteManager.getInstance();
        YtdlMethods ytdl = YtdlMethods.getInstance();
        public static Action<Page> changeFrame;
        public Bolumler(Anime anime,Action<Page> frame)
        {
            DataContext = anime;
            InitializeComponent();
            this.anime = anime;
            changeFrame = frame; 
            

            episodeList.ItemsSource = anime.episodes;
        }
        private void Download(object sender, RoutedEventArgs e)
        {
            AnimeEpisode episode = (AnimeEpisode)(sender as Button).DataContext;
            anime.episode = episode;
            if (episode.watchurl == "bulunamadi") { MessageBox.Show("Video URL'si Sunucuda Bulunamadi.", "Video URL'si bulunamadi.");
                return;
            }

            ytdl.addToQueue(anime);
            
        }

        private void OpenEpisode(object sender, MouseButtonEventArgs e)
        {
            
            AnimeEpisode episode = (AnimeEpisode)(sender as StackPanel).DataContext;
            anime.episode = episode;
            if (episode.watchurl == "bulunamadi")
            {
                MessageBox.Show("Video URL'si Sunucuda Bulunamadi.", "Video URL'si bulunamadi.");
                return;
            }



            if (!episode.watchurl.StartsWith("{"))
            {
                //Eğer liste halinde gelirse köşeli parantezleri yok et ve izleme URL'ini kullan
               anime.episode.watchurl = episode.watchurl.Replace("[", "").Replace("]", "");
               ytdl.openWithmpv(anime);
            } else {
                //Eğer Dictionary döndürürse Fansub/Alternatif Seçme Ekranını Aç
                Dictionary<string,object> obje = JsonConvert.DeserializeObject<Dictionary<string, object>>(episode.watchurl);
                AlternatifSecici secici = new AlternatifSecici(obje,anime);
                changeFrame(secici);
            }





        }
    }
    public class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "bulunamadi")
            {
                return "Visible";
            }

            return "Hidden";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
