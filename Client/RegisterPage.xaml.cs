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
using MySql.Data.MySqlClient;

namespace Client
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private MySqlConnection connection;
        public RegisterPage()
        {
            InitializeComponent();
            connection = Helper.MySQLHelper.getConnection("server=127.0.0.1;uid=root;password=123abc;database=tipdatabase;");
            

        }
        private void loginClick_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Uri uri = new Uri("LoginPage.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {

            if (login.Text.Length < 5)
                MessageBox.Show("Podany login jest za krótki. Login musi mieć minimum 5 znaków!");
            else
            {  
                if (Helper.MySQLHelper.findLogin(login.Text, connection))
                {
                    MessageBox.Show("Podany login już istnieje podaj inny!");
                    login.Clear();
                    password.Clear();
                    password2.Clear();               
                }
                else
                {
                    if (password.ToString().Trim() != password2.ToString().Trim())
                    {
                        MessageBox.Show("Podane hasła różnia się");
                        password.Clear();
                        password2.Clear();
                    }
                    else
                    {
                        if (!Helper.MySQLHelper.insertUser(new Client.User(login.Text, Helper.GlobalHelper.getMD5(password.Password), Helper.GlobalHelper.GetLocalIPAddress()), connection))
                            MessageBox.Show("Wstawienie do bazy nie powiodło się");
                        else
                        {
                            MessageBox.Show("Pomyślnie dodano do bazy");
                            Uri uri = new Uri("LoginPage.xaml", UriKind.Relative);
                            this.NavigationService.Navigate(uri);
                        }
                    }

                 }
            }
        }

    }
}
