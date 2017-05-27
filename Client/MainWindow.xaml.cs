using Client.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            init();
        }


        private void init()
        {
            GlobalMemory.serverAddressIP = "192.168.1.8:11885";
            if (File.ReadAllText("file.txt") != "")
            {
                GlobalMemory._user = GlobalHelper.jsonToUser(File.ReadAllText("file.txt"));
                Uri uri = new Uri("Menu.xaml", UriKind.RelativeOrAbsolute);
                this.NavigationService.Navigate(uri);
            }
            this.ShowsNavigationUI = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
       
    }

}

