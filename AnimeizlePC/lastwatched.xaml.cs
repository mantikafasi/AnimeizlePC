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
    /// lastwatched.xaml etkileşim mantığı
    /// </summary>
    public partial class lastwatched : Page
    {
        SQLiteManager manager;
        YtdlMethods ytdl;
        public lastwatched()
        {
            manager=SQLiteManager.getInstance();
            InitializeComponent();
            var veriler = manager.GetData();



            listview.ItemsSource = veriler;
            ytdl = YtdlMethods.getInstance();


        }

        private void OpenLastWatched(object sender, MouseButtonEventArgs e)
        {
            Anime clickeditem = (Anime)(sender as StackPanel).DataContext;
            ytdl.openWithmpv(clickeditem);

        }
        

        private void DeleteLastWatched(object sender, RoutedEventArgs e)
        {

           manager.DeleteData((sender as Anime));
        }
    }
}
