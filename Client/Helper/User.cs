﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class User
    {
    //    public int ID { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string ipAddress { get; set; }
        public bool status { get; set; }
        public User()        
        {
        }
        public User(string login, string password, string ip,bool status)
        {
            this.login = login;
            this.password = password;
            this.ipAddress = ip;
            this.status = status;
        }
        public override string ToString()
        {
            return login;
        }
    }
}
