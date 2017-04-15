using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Helper
{
    public class OnlineUser
    {
        public string login { get; set; }
        public string ipAddress { get; set; }

        public OnlineUser(string login, string ipAddress)
        {
            this.login = login;
            this.ipAddress = ipAddress;
        }
    }
}