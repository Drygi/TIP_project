using Client.Helper;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
        private UdpUser clientMessage,clientVoice;
        private WaveIn waveSource;
        private WaveFileWriter waveFile;
        private WaveOut waveOut;
        private int portMessage, portVoice;
        public Menu()
        {
            InitializeComponent();
            loginName.Text += GlobalMemory._user.login;
            initialize();
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
                if (clientMessage == null )
                {
                    clientMessage = UdpUser.ConnectTo(GlobalMemory.onlineUsers[listBoxItems.SelectedIndex].ipAddress,portMessage);
                    clientMessage.Send("INVITE");
            }
                else
                    MessageBox.Show("Nie możesz prowadzić dwóch rozmów na raz, najpierw zakończ obecną rozmowę!");
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
            if (Helper.GlobalHelper.messageBoxYesNO("Czy na pewno chcesz zakończyć rozmowę?"))
                clientMessage.Send("BYE");
        }

        private void initialize()
        {
            portMessage = 32123;
            portVoice = 32130;
            waveOut = new WaveOut();
            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(44100, 1);
            waveSource.BufferMilliseconds = 100;
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
            waveFile = new WaveFileWriter("call.wav", waveSource.WaveFormat);
         
            var serverMessage = new UdpListener(new IPEndPoint(IPAddress.Parse(GlobalMemory._user.ipAddress), portMessage));
            var serverVoice  = new UdpListener(new IPEndPoint(IPAddress.Parse(GlobalMemory._user.ipAddress), portVoice));
            Task.Factory.StartNew(async() => 
            {
                    while (true)
                    {
                        var received = await serverMessage.Receive();
                        messageCase(received);
                        
                    }
            });
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var receivedVoice = await serverVoice.ReceiveVoice();
                    playVoice(receivedVoice);
                }
            });
        }
        private void removeAcutalUserFromList(User user)
        {
            for (int i = 0; i < Helper.GlobalMemory.onlineUsers.Count; i++)
            {
                if (Helper.GlobalMemory.onlineUsers[i].login.ToUpper() == user.login.ToUpper())
                    Helper.GlobalMemory.onlineUsers.RemoveAt(i);
            }
        }
        private void playVoice(ReceivedVoice _receivedvoice)
        {
          IWaveProvider provider = new RawSourceWaveStream(_receivedvoice.Message, new WaveFormat(44100, 1));
           waveOut.Init(provider); 
           waveOut.Play();
        }    
        private void messageCase(Received _received)
        {
            string _message = _received.Message;
            string _login  = GlobalHelper.getClientByIP(GlobalMemory.onlineUsers, _received.Sender.Address.ToString().Trim());
            switch (_message)
            {
                case "INVITE":
                {
                        clientMessage = UdpUser.ConnectTo(_received.Sender.Address.ToString(), portMessage);
                        if (GlobalHelper.messageBoxYesNO("Dzwoni " + _login + " czy chcesz odebrać?"))
                        {
                            clientVoice = UdpUser.ConnectTo(_received.Sender.Address.ToString(), portVoice);
                            clientMessage.Send("ACK");
                            while (clientVoice == null);
                            waveSource.StartRecording();
                        }
                        else
                        {
                            clientMessage.Send("CANCEL");
                            clientMessage = null;
                        }
                    break;
                }
                case "ACK":
                {
                        // tutaj bedzie trzeba zrobić jakąs animacje ze rozmowa jest
                        while (clientVoice == null);
                        waveSource.StartRecording();
                        MessageBox.Show("Połączenie zostało odebrane");
                    break;
                }
                case "BYE":
                {
                        clientMessage.Send("BYE");
                        waveSource.StopRecording();
                        Thread.Sleep(1000);
                        clientVoice = null;
                        clientMessage = null;
                        MessageBox.Show("Połączenie zostało zakończone");
                    break;
                }
                case "CANCEL":
                {
                        MessageBox.Show("Połączenie zostało odrzucone");
                        clientMessage = null;
                    break;
                }
                default:
                    break;
            }
        }
       private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveSource != null)
            {
               clientVoice.SendBytes(e.Buffer);
            }
        }      
        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }
            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }
        
    }

}
