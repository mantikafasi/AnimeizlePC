using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnimeizlePC
{
    /// <summary>
    /// Interaction logic for downloaded.xaml
    /// </summary>
    public partial class downloaded : Page
    {
        SQLiteManager manager;
        YtdlMethods ytdl;
        public downloaded()
        {
            InitializeComponent();
            manager = SQLiteManager.getInstance();


            listview.ItemsSource = manager.GetData(SQLiteManager.dataType.downloadedAnime);
            ytdl = YtdlMethods.getInstance();


        }
        private void openDownloaded(object sender, MouseButtonEventArgs e)
        {
            Anime clickeditem = (Anime)(sender as StackPanel).DataContext;
            ytdl.openWithmpv(clickeditem);

        }
    }
}
