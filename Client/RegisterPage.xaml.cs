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
using Client.Helper;

namespace Client
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();    
        }
        private void loginClick_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Uri uri = new Uri("LoginPage.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }

        private async void registerButton_Click(object sender, RoutedEventArgs e)
        {
            var s = new OnlineUser();
            if (login.Text.Length < 5)
                MessageBox.Show("Podany login jest za krótki. Login musi mieć minimum 5 znaków!");
            else
            {
                s.login = login.Text;
                s.ipAddress = " ";

                if (!await Helper.APIHelper.findLogin(s))
                {
                    MessageBox.Show("Podany login już istnieje podaj inny!");
                    login.Clear();
                    password.Clear();
                    password2.Clear();               
                }
                else
                {
                    if (password.Password.Trim() != password2.Password.Trim())
                    {
                        MessageBox.Show("Podane hasła różnia się");
                        password.Clear();
                        password2.Clear();
                    }
                    else
                    {
                        var user = new User(login.Text, Helper.GlobalHelper.getMD5(password.Password), Helper.GlobalHelper.GetLocalIPAddress(), false);
                        if (!await Helper.APIHelper.register(user))
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
