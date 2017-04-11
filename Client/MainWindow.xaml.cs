using MySql.Data.MySqlClient;
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
        private MySqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            this.ShowsNavigationUI = false;
            connection = Helper.MySQLHelper.getConnection("server=127.0.0.1;uid=root;password=123abc;database=tipdatabase;");
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Helper.GlobalMemory._user != null)
                Helper.MySQLHelper.updateStatus(false, Helper.GlobalMemory._user.login, connection);
        }

    }
}
