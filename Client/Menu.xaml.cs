using System;
using System.Collections.Generic;
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

namespace Client
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private List<User> users;
        public Menu()
        {
            InitializeComponent();
            loginName.Text += Helper.GlobalMemory._user.login;
            users = new List<User>();
            
        }


        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Helper.GlobalHelper.messageBoxYesNO("Czy na pewno chcesz się wylogować?"))
            {
                Helper.GlobalMemory._user = null;
                Uri uri = new Uri("LoginPage.xaml", UriKind.Relative);
                this.NavigationService.Navigate(uri);
            }
            
        }

        private void onlineUsers_Click(object sender, RoutedEventArgs e)
        {
            //  generateContact();
            listBoxItems.ItemsSource = Helper.GlobalHelper.getOnlineAddressesIP();
            //MessageBox.Show(listBoxItems.SelectedIndex.ToString());
        }
        private void generateContact()
        {
            for (int i = 0; i < 10; i++)
            {
                users.Add(new User("Kontakt " + (i + 1).ToString(), "haslo", "ip"));
            }
            listBoxItems.ItemsSource = users;
        }
    }
}
