using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Helper
{
    public static class GlobalHelper
    {
        public static string getMD5(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.UTF8.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                str.Append(result[i].ToString("x2"));
            }

            return str.ToString();
        }

        public static string GetLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
        public static bool messageBoxYesNO(string txt)
        {
            bool answer = false;
            MessageBoxResult result = MessageBox.Show(txt,"", MessageBoxButton.YesNo);
                switch(result)
                {
                case MessageBoxResult.Yes:
                    answer = true;
                        break;
                case MessageBoxResult.No:
                    answer = false;
                        break;
                  }
            return answer;
        }

        public static List<User> removeClientFromUsers(List<User>users,User user)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].login == user.login)                
                    users.RemoveAt(i);               
            }       
            return users;
        }
    }
}
