using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helper
{
    public static class MySQLHelper
    {
        public static MySqlConnection getConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public static bool insertUser(User user, MySqlConnection conn)
        {
            bool returned = true;

            try
            {
                conn.Open();
                string insert = "INSERT INTO users (login,password,ipAddress) VALUES (@login,@password,@ipAddress)";
                MySqlCommand cmd = new MySqlCommand(insert, conn);

                cmd.Parameters.AddWithValue("@login", user.login);
                cmd.Parameters.AddWithValue("@password",user.password);
                cmd.Parameters.AddWithValue("@ipAddress", user.ipAddress);
 
                var r = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                returned = false;
            }
            finally
            {

                if (conn != null)
                {
                    conn.Close();
                }
            }
            return returned;
        }

        public static bool findLogin(string login, MySqlConnection conn)
        {
            bool returned = true;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE Login=@log;", conn);

                cmd.Parameters.AddWithValue("@log", login);

                var result = cmd.ExecuteReader();

                if (result.HasRows)
                {
                    returned = true;
                }
                else
                    returned = false;

            }
            catch (Exception ex)
            {
                returned = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return returned;
        }

       //sprawdzanie poprawności danych oraz update IP 
        public static bool checkCorrectAccount(string login, string password, MySqlConnection conn, out User user)
        {
            bool returned = true;
            User objUser = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE login=@log AND password= @pass;", conn);
                cmd.Parameters.AddWithValue("@log", login);
                cmd.Parameters.AddWithValue("@pass", password);
               
                var result = cmd.ExecuteReader();
                if (result.HasRows)
                {
                    result.Read();
                    objUser = new User();
                    objUser.ID = Convert.ToInt16(result[0]);
                    objUser.login = result[1].ToString();
                    objUser.password = result[2].ToString();
                    objUser.ipAddress = GlobalHelper.GetLocalIPAddress();
                    
                    MySqlCommand cmd2 = new MySqlCommand("UPDATE users SET ipAddress = @thisPrice WHERE login=@login", conn);
                    cmd.Parameters.AddWithValue("@ipAddress", objUser.ipAddress);
                    cmd.Parameters.AddWithValue("@login", login);
                     returned = true;
        
                }
                else returned = false;

            }
            catch (Exception ex)
            {
                returned = false;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            user = objUser;
            return returned;
        }
    }
}
