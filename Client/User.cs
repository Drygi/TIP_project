using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class User
    {
        public string login { get; set; }
        public string password { get; set; }
        public string ipAddress { get; set; }
        public User()        
        {
        }
        public User(string login, string password, string ip)
        {
            this.login = login;
            this.password = password;
            this.ipAddress = ip;
        }
        public override string ToString()
        {
            return login;
        }
    }
}
