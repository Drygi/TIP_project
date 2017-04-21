using Client.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            GlobalMemory.serverAddressIP = Microsoft.VisualBasic.Interaction.InputBox("Wpisz adres IP serwera wraz z portem!");
            this.ShowsNavigationUI = false;
        }
        private async void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Helper.GlobalMemory._user != null)
                await Helper.APIHelper.logout(Helper.GlobalMemory._user);
        }

    }
}
