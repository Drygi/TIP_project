using Helper;
using MySql.Data.MySqlClient;
using Server.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Server
{
    public class UserManager
    {
       public List<User> users  {get;set;}
        private MySqlConnection connection;
        public UserManager()
        {
            connection = MySQLHelper.getConnection("server=127.0.0.1;uid=root;password=123abc;database=tipdatabase;");
             users = new List<User>();
            users = MySQLHelper.getOnlineUsers(connection);
            
        }
    
        
          
    }
}