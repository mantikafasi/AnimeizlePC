using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// DetailsPage.xaml etkileşim mantığı
    /// </summary>
    public partial class DetailsPage : Window
    {
        Anime anime;
        YtdlMethods ytdl;
        SQLiteManager sqlitemanager;
        public DetailsPage(Anime anim)
        {
            InitializeComponent();
            this.DataContext = anim;
            RequestManager api = RequestManager.getInstance();
            anime = api.getDetails(anim);
            ytdl = YtdlMethods.getInstance();
            sqlitemanager = SQLiteManager.getInstance();

            episodeList.ItemsSource = anime.episodes;
        }

        private void Download(object sender, RoutedEventArgs e)
        {
            AnimeEpisode episode = (AnimeEpisode)(sender as Button).DataContext;
            anime.episode = episode;
            if (episode.watchurl != "bulunamadi"){ytdl.addToQueue(anime);}else{MessageBox.Show("Video URL'si Sunucuda Bulunamadi.", "Video URL'si bulunamadi.");}
            
        }

        private void OpenEpisode(object sender, MouseButtonEventArgs e)
        {
            AnimeEpisode episode = (AnimeEpisode)(sender as StackPanel).DataContext;
            episode.watchurl =episode.watchurl.Replace("[", "").Replace("]", "");
            anime.episode = episode;
            if (episode.watchurl != "bulunamadi")
            {
                sqlitemanager.AddData(anime);
                ytdl.openWithmpv(anime);
            }
            else
            {
                MessageBox.Show("Video URL'si Sunucuda Bulunamadi.","Video URL'si bulunamadi.");
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
