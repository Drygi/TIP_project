using MySql.Data.MySqlClient;
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
        private MySqlConnection connection;
        public Menu()
        {
            InitializeComponent();
            loginName.Text += Helper.GlobalMemory._user.login;
            connection = Helper.MySQLHelper.getConnection("server=127.0.0.1;uid=root;password=123abc;database=tipdatabase;");
            users = new List<User>();       
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Helper.GlobalHelper.messageBoxYesNO("Czy na pewno chcesz się wylogować?"))
            {
                Helper.MySQLHelper.updateStatus(false,Helper.GlobalMemory._user.login, connection);
                Helper.GlobalMemory._user = null;
                Uri uri = new Uri("LoginPage.xaml", UriKind.Relative);
                this.NavigationService.Navigate(uri);
            }
            
        }

        private void onlineUsers_Click(object sender, RoutedEventArgs e)
        {
            Helper.GlobalMemory.users = Helper.MySQLHelper.getOnlineUsers(connection);
            listBoxItems.ItemsSource = Helper.GlobalMemory.users;   
        }

        private void callButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxItems.SelectedIndex == -1)
            {
                MessageBox.Show("Nie wybrano żadnego użytkownika");
            }
            else
            {
                MessageBox.Show(Helper.GlobalMemory.users[listBoxItems.SelectedIndex].login);
            }
        }
    }
}
