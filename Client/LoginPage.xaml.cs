using Client.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {

            if (login.Text.Trim().Length<5 || password.Password.Trim().Length<5)
            {
                MessageBox.Show("Podany login lub hasło jest za krótkie! Minimum 5 znaków!");
                login.Text = "";
                password.Password = "";
            }
            else
            {
                User user = new User(login.Text, Helper.GlobalHelper.getMD5(password.Password), 
                Helper.GlobalHelper.GetLocalIPAddress(), true);

                if (await Helper.APIHelper.login(user))
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
        }

        private void registerClick_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Uri uri = new Uri("RegisterPage.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);   
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}