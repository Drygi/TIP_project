﻿using Client.Helper;
using Client.Properties;
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
using System.Timers;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
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
        private UdpUser clientMessage, clientVoice;
        private UdpListener serverMessage, serverVoice;
        private WaveIn waveSource;
        private WaveFileWriter waveFile;
        private WaveOut waveOut;
        private int portMessage, portVoice;
        //  private WaveInProvider waveInProvider;
        public Menu()
        {
            InitializeComponent();
            initialize();
            loginName.Text += GlobalMemory._user.login;
            startServerListening();
            callEndButton.Visibility = Visibility.Hidden;
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalHelper.messageBoxYesNO("Czy na pewno chcesz się wylogować?"))
            {
                if (await APIHelper.logout(Helper.GlobalMemory._user))
                {
                    File.WriteAllText("file.txt", "");
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
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
            if (GlobalMemory.onlineUsers.Count == 0)
                MessageBox.Show("Brak kontaków online");
            else
                listBoxItems.ItemsSource = Helper.GlobalMemory.onlineUsers;
        }
       private void callButton_Click(object sender, RoutedEventArgs e)
       {
            if (listBoxItems.SelectedIndex == -1)
                MessageBox.Show("Nie wybrano żadnego użytkownika");
            else
                if (clientMessage == null)
                {
                    callButton.Visibility = Visibility.Hidden;
                    callEndButton.Visibility = Visibility.Visible;
                    clientMessage = UdpUser.ConnectTo(GlobalMemory.onlineUsers[listBoxItems.SelectedIndex].ipAddress, portMessage);
                    clientMessage.Send(MySIP.INVITE);
                }
       }
        private async void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (await Helper.APIHelper.logout(Helper.GlobalMemory._user))
            {
                File.WriteAllText("file.txt", "");
                Application.Current.Shutdown();
            }
            else
                MessageBox.Show("Zamknięcie nie powiodło się");

        }

        private void callEndButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientVoice != null)
            {
                if (Helper.GlobalHelper.messageBoxYesNO("Czy na pewno chcesz zakończyć rozmowę?"))
                {
                    clientMessage.Send(MySIP.BYE);
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
            }
            else
                MessageBox.Show("Nie prowadzisz obecnie żadnej rozmowy");
        }
        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {

            if (Helper.GlobalHelper.messageBoxYesNO("Czy na pewno chcesz usunąć swoje konto?"))
            {
                if (await APIHelper.deleteUser(GlobalMemory._user))
                {
                    MessageBox.Show("Konto zostało usunięte pomyślnie");
                    File.WriteAllText("file.txt","");
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
                else
                    MessageBox.Show("Coś poszło nie tak");
            }
        }
       
        private void initialize()
        {
            //wywolanie Clicka onlineUsers
            ButtonAutomationPeer peer = new ButtonAutomationPeer(onlineUsers);
            IInvokeProvider   invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
            portMessage = 32123;
            portVoice = 32120;
            waveOut = new WaveOut();
            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(44100, 2);
            waveSource.BufferMilliseconds = 100;
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

            if (File.ReadAllText("file.txt")=="")
                File.WriteAllText("file.txt", GlobalHelper.userToJson(GlobalMemory._user));
            


            waveFile = new WaveFileWriter("call.wav", waveSource.WaveFormat);
           // waveInProvider = new WaveInProvider(waveSource);
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
            IWaveProvider provider = new RawSourceWaveStream(_receivedvoice.Message, new WaveFormat(44100, 2));
            waveOut.Init(provider);
            waveOut.Play();
            provider = null;

        }
        private void messageCase(Received _received)
        {
            string _message = _received.Message;
            string _login = GlobalHelper.getClientByIP(GlobalMemory.onlineUsers, _received.Sender.Address.ToString().Trim());

            switch (_message)
            {
                case "INVITE":
                    {
                            clientMessage = UdpUser.ConnectTo(_received.Sender.Address.ToString(), portMessage);
                        
                            if (GlobalHelper.messageBoxYesNO("Dzwoni " + _login + " czy chcesz odebrać?"))
                            {
                                clientVoice = UdpUser.ConnectTo(_received.Sender.Address.ToString(), portVoice);
                                clientMessage.Send(MySIP.ACK);
                           
                              //  callButton.Visibility = Visibility.Hidden;
                                waveSource.StartRecording();
                            }
                            else
                            {
                                clientMessage.Send(MySIP.CANCEL);        
                                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                                Application.Current.Shutdown();
                            }
                        break;
                    }
                case "ACK":
                    {
                            clientVoice = UdpUser.ConnectTo(_received.Sender.Address.ToString(), portVoice);
                           // callButton.Visibility = Visibility.Hidden;

                            waveSource.StartRecording();
                            MessageBox.Show("Połączenie zostało odebrane");
                        break;
                    }
                case "BYE":
                    {
                            //waveSource.StopRecording();
                            MessageBox.Show("Połączenie zostało zakończone");
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown(); 
                        break;
                    }
                case "CANCEL":
                    {
                            MessageBox.Show("Połączenie zostało odrzucone");
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                        break;
                    }
                default:
                    break;
            }
        }
        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            //   waveInProvider = new WaveInProvider(waveSource); ;
            if (waveSource != null)
            {
                clientVoice.SendBytes(e.Buffer);

                // waveFile.WriteAsync(e.Buffer, 0, e.BytesRecorded);
                //waveFile.FlushAsync();
                //  waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
                

                //   waveInProvider.Read(e.Buffer, 0, e.BytesRecorded);
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
        private void startServerListening()
        {
            serverVoice = new UdpListener(new IPEndPoint(IPAddress.Parse(GlobalMemory._user.ipAddress), portVoice));
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var receivedVoice = await serverVoice.ReceiveVoice();
                    playVoice(receivedVoice);
                }
            });
            serverMessage = new UdpListener(new IPEndPoint(IPAddress.Parse(GlobalMemory._user.ipAddress), portMessage));
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await serverMessage.Receive();
                    messageCase(received);
                }
            });
        }
    }
}
