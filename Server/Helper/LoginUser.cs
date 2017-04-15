using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Helper
{
    public class LoginUser
    {
        public string login { get; set; }
        public string password { get; set; }
        public string ipAddress { get; set; }
        public bool status { get; set; }
        public LoginUser(string login, string password, string ipAddress, bool status)
        {
            this.login = login;
            this.password = password;
            this.ipAddress = ipAddress;
            this.status = status;
        }
    }
}