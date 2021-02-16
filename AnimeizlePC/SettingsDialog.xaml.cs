using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace AnimeizlePC
{
    /// <summary>
    /// SettingsDialog.xaml etkileşim mantığı
    /// </summary>
    public partial class SettingsDialog : Window
    {
        Settings set;
        public SettingsDialog()
        {
            InitializeComponent();
            set = App.settings;
            DataContext =set;
        }


        private void SelectSaveLocation(object sender, RoutedEventArgs e)
        {
            
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog()==System.Windows.Forms.DialogResult.OK) {
                set.saveLocation = dialog.SelectedPath;

                set.NotifyPropertyChanged("saveLocation");
                

            }
            
        }
        private void kaydet(object sender,RoutedEventArgs e)
        {
            set.Save();
            DialogResult = true;

        }
        private void OpenSaveLocation(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe",set.saveLocation);


        }

        private void changeTheme(object sender, RoutedEventArgs e)
        {
            set.darkmode = ((bool)(sender as ToggleButton).IsChecked);
            App app = (App)System.Windows.Application.Current;
            
            if (set.darkmode)
            {
                app.ChangeTheme(new Uri("pack://application:,,,/Themes/DarkTheme.xaml"));
            } else
            {
                app.ChangeTheme(new Uri("pack://application:,,,/Themes/LightTheme.xaml"));
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
