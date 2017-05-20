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
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                   return ip.ToString();
                }
            }
            return "";
        }
        public static bool messageBoxYesNO(string txt)
        {
            MessageBoxResult result = MessageBox.Show(txt,"", MessageBoxButton.YesNo);
                switch(result)
                {
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                  }
            return false;
        }

        public static List<User> removeClientFromUsers(List<User>users,User user)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].login.ToUpper() == user.login.ToUpper())                
                    users.RemoveAt(i);               
            }       
            return users;
        }

        public static string getClientByIP(List<OnlineUser> users,string ip)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].ipAddress == ip)
                    return users[i].login;
            }
            return "";
        }
    }
}
