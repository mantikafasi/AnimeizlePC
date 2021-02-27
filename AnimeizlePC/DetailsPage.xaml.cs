using System;
using System.Collections;
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
using AnimeizlePC.detailspageframes;

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
            DataContext = anim;
            RequestManager api = RequestManager.getInstance();
            anime = api.getDetails(anim);
            ytdl = YtdlMethods.getInstance();
            sqlitemanager = SQLiteManager.getInstance();
            Bolumler page = new Bolumler(anime, changeFrame);
            frame.Navigate(page);

        }

        public void changeFrame(Page obje){if (obje == null){frame.GoBack();}else{frame.Navigate(obje);}}

    }

 
}
