
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Json;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AnimeizlePC
{

    public partial class App : Application
    {

        public App(){

            }

        public ResourceDictionary ThemeDictionary{get { return Resources.MergedDictionaries[0];}}
        public void ChangeTheme(Uri uri)
        {
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });   

        }


        public static Settings settings;
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {}

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            settings = new Settings().Load();
            if (settings.darkmode)
            {
                ChangeTheme(new Uri("pack://application:,,,/Themes/DarkTheme.xaml"));
            }

        }
    }
    public class Settings : SettingsBase, INotifyPropertyChanged
    {

        public bool darkmode { get; set; }
        public string saveLocation { get; set; }
        public string VideoName { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }
    }

    public class SettingsBase {

        public const string FileName = "ayarlar.json";
        public void Save(Settings settings,string fileName = FileName)
        {
            File.WriteAllText(fileName,JsonConvert.SerializeObject(settings));
        }
        public void Save(string fileName = FileName)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
        }
        public Settings Load()
        {
            if (!File.Exists(FileName)) {
                Save();
            }
            Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FileName));
            if (string.IsNullOrEmpty(settings.saveLocation))
            {
                settings.saveLocation= Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }

            if (string.IsNullOrEmpty(settings.VideoName))
            {
                settings.VideoName = "%bolumadi";
            }

            return settings;
        }
    }
}
