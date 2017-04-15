using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helper
{
    public class OnlineUser
    {
        public string login { get; set; }
        public string ipAddress { get; set; }
        public OnlineUser()
        {
        }
        public OnlineUser(string login, string ipAddress)
        {
            this.login = login;
            this.ipAddress = ipAddress;
        }
        public override string ToString()
        {
            return login;
        }
    }
}
