﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Views
{
    public class User
    {
        public int ID { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string ipAddress { get; set; }
        public bool Status { get; set; }

        public User()
        {
        }
    
    public User(string login,string password, string IP, bool status)
        {
            this.login = login;
            this.password = password;
            this.ipAddress = IP;
            this.Status = status;
        }
        
}
}