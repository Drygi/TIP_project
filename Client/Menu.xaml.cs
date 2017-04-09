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

namespace Client
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private List<User> users;
        public Menu()
        {
            InitializeComponent();
            users = new List<User>();
            generateContact();
        }

        private void callButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(listBoxItems.SelectedIndex.ToString());
        }
        private void generateContact()
        {
            for (int i = 0; i < 10; i++)
            {
                users.Add(new User("Kontakt " + (i + 1).ToString(), "haslo", "ip"));
            }
            listBoxItems.ItemsSource = users;
        }
    }
}
