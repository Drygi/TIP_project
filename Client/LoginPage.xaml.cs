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
        private User user;
        public LoginPage()
        {
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            if (!File.Exists("file.txt"))
                File.WriteAllText("file.txt", "");

            if (File.ReadAllText("file.txt") != "")
            {
                GlobalMemory._user = GlobalHelper.jsonToUser(File.ReadAllText("file.txt"));
                Uri uri = new Uri("Menu.xaml", UriKind.Relative);
                this.NavigationService.Navigate(uri);
            }
            else
                user = new User();

        }
        private async void loginButton_Click(object sender, RoutedEventArgs e)
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

        private void registerClick_MouseDown(object sender, MouseButtonEventArgs e)
        {
      
            Uri uri = new Uri("RegisterPage.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
            
        }
    
    }
}