using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Helper
{
    public class RegisterUser
    {
        public string login { get; set; }
        public string password { get; set; }
        public string ipAddress { get; set; }

        public RegisterUser(string login, string password, string ipAddress)
        {
            this.login = login;
            this.password = password;
            this.ipAddress = ipAddress;
        }
    }
}