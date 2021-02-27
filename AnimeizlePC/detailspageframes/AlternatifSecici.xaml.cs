using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
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
    /// Interaction logic for AlternatifSecici.xaml
    /// </summary>
    public partial class AlternatifSecici : Page
    {
        private Dictionary<string, object> episode;
        private Anime anime;
        private JsonObject alternatifler;
        private YtdlMethods ytdl = YtdlMethods.getInstance();

        public AlternatifSecici(Dictionary<string, object> obje,Anime anime)
        {
            this.anime = anime;
            InitializeComponent();
            episode = obje; 
            List<string> fansublar = new List<string>();
            foreach (var objeKey in obje.Keys)
            {
                fansublar.Add(objeKey);  
            }

            FansubLW.ItemsSource = fansublar;


        }
        public void onAlternatifClicked(object sender, EventArgs e)
        {
            string alternatif = (string)(sender as Button).DataContext;
            anime.episode.watchurl= alternatifler[alternatif];
            ytdl.openWithmpv(anime);
        }

        

        private void onFansubClicked(object sender, RoutedEventArgs e)
        {
            string fansub =(string) (sender as Button).DataContext;
            alternatifler =  JsonConvert.DeserializeObject<JsonObject>(episode[fansub].ToString());

            AlternatiflerLW.ItemsSource = new List<String>(alternatifler.Keys.ToList());
        }

        private void onBackPressed(object sender, RoutedEventArgs e)
        {
            Bolumler.changeFrame(null);
        }

        private void Download(object sender, RoutedEventArgs e)
        {
            string alternatif = (string)(sender as Button).DataContext;
            Anime klon = anime.Clone;
            klon.episode.watchurl = alternatifler[alternatif];
            ytdl.addToQueue(klon);
        }
    }
}
