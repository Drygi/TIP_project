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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private MySqlConnection connection;
        private User user;
        public LoginPage()
        {
            InitializeComponent();
            connection = Helper.MySQLHelper.getConnection("server=127.0.0.1;uid=root;password=123abc;database=tipdatabase;");
            user = new User();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (Helper.MySQLHelper.checkCorrectAccount(login.Text, Helper.GlobalHelper.getMD5(password.ToString()), connection, out user))
            {
                Helper.GlobalMemory._user = user;
                Uri uri = new Uri("Menu.xaml", UriKind.Relative);
                this.NavigationService.Navigate(uri);
            }
            else
            {
                MessageBox.Show("Bląd podczas logowania");
                login.Clear();
                password.Clear();
            }
        }

        private void registerClick_MouseDown(object sender, MouseButtonEventArgs e)
        {
      
            Uri uri = new Uri("RegisterPage.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
            
        }
    }
}
