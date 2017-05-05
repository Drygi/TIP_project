using Client.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
        UdpUser client;
        public Menu()
        {
            InitializeComponent();
            loginName.Text += GlobalMemory._user.login;  
            startListening();

        }
        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Helper.GlobalHelper.messageBoxYesNO("Czy na pewno chcesz się wylogować?"))
            {
                if (await Helper.APIHelper.logout(Helper.GlobalMemory._user))
                {
                    GlobalMemory._user = null;
                    Uri uri = new Uri("LoginPage.xaml", UriKind.Relative);
                    this.NavigationService.Navigate(uri);
                }
                else
                    MessageBox.Show("Coś poszło nie tak");
            }
        }

        private async void onlineUsers_Click(object sender, RoutedEventArgs e)
        {
            listBoxItems.ItemsSource = null;
            GlobalMemory.onlineUsers = await APIHelper.getOnlineUsers();
            removeAcutalUserFromList(GlobalMemory._user);
            if (Helper.GlobalMemory.onlineUsers.Count == 0)
                MessageBox.Show("Brak kontaków online");
            else
                listBoxItems.ItemsSource = Helper.GlobalMemory.onlineUsers;

        }

        private void callButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxItems.SelectedIndex == -1)
                MessageBox.Show("Nie wybrano żadnego użytkownika");
            else
                if (client == null)
            {
                client = UdpUser.ConnectTo(GlobalMemory.onlineUsers[listBoxItems.SelectedIndex].ipAddress, 32123);
                client.Send("INVITE");
            }
            else
                MessageBox.Show("Nie możesz prowadzić dwóch rozmów na raz, najpierw zakończ obecną rozmowę!");
        }

        public void removeAcutalUserFromList(User user)
        {
            for (int i = 0; i < Helper.GlobalMemory.onlineUsers.Count; i++)
            {
                if (Helper.GlobalMemory.onlineUsers[i].login.ToUpper() == user.login.ToUpper())
                    Helper.GlobalMemory.onlineUsers.RemoveAt(i);
            }
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Helper.GlobalHelper.messageBoxYesNO("Czy na pewno chcesz usunąć swoje konto?"))
            {
                
                if (await APIHelper.deleteUser(GlobalMemory._user))
                {
                    MessageBox.Show("Konto zostało usunięte pomyślnie");
                    Uri uri = new Uri("LoginPage.xaml", UriKind.Relative);
                    this.NavigationService.Navigate(uri);
                    GlobalMemory._user = null;
                }
                else
                    MessageBox.Show("Coś poszło nie tak");
            }
        }


        private void callEndButton_Click(object sender, RoutedEventArgs e)
        {
            if(Helper.GlobalHelper.messageBoxYesNO("Czy na pewno chcesz zakończyć rozmowę?"))
            {
                client.Send("BYE");
                client = null;
            }

        }

        private void startListening()
        {
            var server = new UdpListener(new IPEndPoint(IPAddress.Parse(GlobalMemory._user.ipAddress), 32123));

            //start listening for messages and copy the messages back to the client
            Task.Factory.StartNew(async() => {
                    while (true)
                    {
                        var received = await server.Receive();
                 
                    MessageBox.Show(received.Sender.Address.ToString() + ": " + received.Message);
                    messageCase(received);

                    //  if (received.Message == "quit")
                    //        break;
                    }
                                        });

        }

        private void messageCase(Received _received)
        {
            string _message = _received.Message;
            string _login  = GlobalHelper.getClientByIP(GlobalMemory.onlineUsers, _received.Sender.Address.ToString().Trim());
            switch (_message)
            {
                case "INVITE":
                {
                        client = UdpUser.ConnectTo(_received.Sender.Address.ToString(), 32123);
                     
                        if (GlobalHelper.messageBoxYesNO("Dzwoni " + _login + " czy chcesz odebrać?"))
                        {
                            client.Send("ACK");
                        }
                        else
                        {
                            client.Send("CANCEL");
                            client = null;
                        }
                        break;
                }
                case "ACK":
                {
                         // tutaj bedzie trzeba zrobić jakąs animacje ze rozmowa jest
                        MessageBox.Show("Połączenie zostało odebrane");
                    break;
                }
                case "BYE":
                {
                        MessageBox.Show("Połączenie zostało zakończone");
                        client = null;
                    break;
                }
                case "CANCEL":
                {
                        MessageBox.Show("Połączenie zostało odrzucone");
                        client = null;
                    break;
                }

                default:
                    break;
            }
        }
}
}
