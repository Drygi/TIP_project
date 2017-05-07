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
        private UdpUser client;
        private WaveIn waveSource;
        private WaveFileWriter waveFile;
        private WaveOut waveOut;
        private Stream output;

        public Menu()
        {
            InitializeComponent();
            loginName.Text += GlobalMemory._user.login;
            waveOut = new WaveOut();
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
                sendVoice();
                 //client.Send("INVITE");
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
              //  client.Send("BYE");
             //   callEndButton.IsEnabled = false;
                waveSource.StopRecording();
                //  client = null;
            }

        }

        private void startListening()
        {
            waveFile = null;
            waveSource = null;
           
            var server = new UdpListener(new IPEndPoint(IPAddress.Parse(GlobalMemory._user.ipAddress), 32123));

            //start listening for messages and copy the messages back to the client
            Task.Factory.StartNew(async() => {
                    while (true)
                    {
                       // var received = await server.Receive();
                        //messageCase(received);
                        var receivedVoice = await server.ReceiveVoice();
                        Console.WriteLine("LOL");
                        playVoice(receivedVoice);

                    //  if (received.Message == "quit")
                    //        break;
                    }
                                        });

        }
        private void playVoice(ReceivedVoice _receivedvoice)
        {
            IWaveProvider provider = new RawSourceWaveStream(_receivedvoice.Message, new WaveFormat());
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
                        client = UdpUser.ConnectTo(_received.Sender.Address.ToString(), 32123);
                     
                        if (GlobalHelper.messageBoxYesNO("Dzwoni " + _login + " czy chcesz odebrać?"))
                        {
                            client.Send("ACK");
                            sendVoice();
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
                        sendVoice();
                    break;
                }
                case "BYE":
                {
                        MessageBox.Show("Połączenie zostało zakończone");
                        callEndButton.IsEnabled = false;
                        waveSource.StopRecording();
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

  
        void sendVoice()
        {
            callButton.IsEnabled = false;

            callEndButton.IsEnabled = true;

            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(44100, 1);

            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
            //zapis dzwieku
            waveFile = new WaveFileWriter("Test0001.wav", waveSource.WaveFormat);

            
           // waveFile = new WaveFileWriter(output, waveSource.WaveFormat);
            waveSource.StartRecording();

        }
 
        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {

            if (waveFile != null)
            {
               
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                client.SendBytes(e.Buffer);
                waveFile.Flush();

                
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
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

            callButton.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            output = new WaveFileReader("Test0001.wav");
            sendInParts(output);
            // client.SendBytes(ReadFully(output));
            //sendVoice();
        }
        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 *1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private void sendInParts(Stream input)
        {
            byte[] buffer =  ReadFully(input);
            byte[] buffer2 = new byte[6500];
            int counter = 0;

            for (int i = 0; i < buffer.Length; i++)
            {

                buffer2[counter] = buffer[i];
                counter++;
                if (counter == 6500 || i==buffer.Length-1)
                {
                    counter = 0;
                    client.SendBytes(buffer2);
                    buffer2 = new byte[6500];
                }
            }
        }
    }

}
