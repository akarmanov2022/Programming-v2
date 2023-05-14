using System;
using System.IO;
using System.Windows;
using static System.Environment;

namespace Contacts
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Хранит путь к папке с сохранениями.
        /// </summary>
        public static readonly string DefaultSavePath = 
            Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), nameof(Contacts));
        
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (Directory.Exists(DefaultSavePath)) return;
                Directory.CreateDirectory(DefaultSavePath);

                var filePath = Path.Combine(DefaultSavePath, "save.json");

                if (File.Exists(filePath)) return;
                File.Create(filePath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK);
            }
        }
        
    }
}