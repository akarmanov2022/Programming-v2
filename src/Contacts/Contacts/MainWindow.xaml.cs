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
            
            ObservableCollection<Contact?> contacts =
                Serializer<ObservableCollection<Contact>>.FromJson(App.DefaultSavePath)
                ?? new ObservableCollection<Contact?>();
            DataContext = new MainVm(contacts);
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            TbName.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            TbEmail.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            TbPhone.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }
    }
}