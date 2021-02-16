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
    /// SearchAnime.xaml etkileşim mantığı
    /// </summary>
    public partial class SearchAnime : Page
    {
        RequestManager api;
        List<Anime> list;
        public SearchAnime()
        {
            InitializeComponent();
            api = RequestManager.getInstance();
            list = api.Search(null);

            animegrid.ItemsSource = list;
        }

    
    private void searchbox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Disable new lines when pressing enter without shift
        if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
        {
            list = api.Search(searchbox.Text);
            animegrid.ItemsSource = list;
        }
    }



    private void StackPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Anime anime = (Anime)(sender as StackPanel).DataContext;
        DetailsPage page = new DetailsPage(anime);
        page.Show();
    }
}



}
