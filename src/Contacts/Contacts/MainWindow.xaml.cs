using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Contacts.Model;
using Contacts.Services;
using Contacts.ViewModel;

namespace Contacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var contacts =
                Serializer<ObservableCollection<Contact>>.FromJson(App.DefaultSavePath)
                ?? new ObservableCollection<Contact>();
            DataContext = new MainVm(contacts);
        }
    }
}