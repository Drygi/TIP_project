using Helper;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Server.Helper;
using Server.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;


namespace Server.Controllers
{
    [RoutePrefix("api")]
    public class UsersController : ApiController
    {
        MySqlConnection connection = MySQLHelper.getConnection("server=127.0.0.1;uid=root;password=123abc;database=tipdatabase;");
      

        // POST api/user
        [AllowAnonymous]
        [Route("user")]     
        public IHttpActionResult Register(User userModel)
        {
            var responseDict = new Dictionary<string, User>();
            HttpResponseMessage responseMsg = Request.CreateResponse
                (HttpStatusCode.OK, responseDict, new MediaTypeHeaderValue("application/json"));    
            IHttpActionResult response = ResponseMessage(responseMsg);
            if (MySQLHelper.findLogin(userModel.login, connection))
                return Unauthorized();
            else
                 MySQLHelper.insertUser(userModel, connection);
            
            return response;       
        }

        //GET api/findLogin
        [Route("findLogin")]
        [AllowAnonymous]
        public IHttpActionResult findLoign(OnlineUser user)
        {
            if (MySQLHelper.findLogin(user.login, connection))
                return Ok();
            else
                return Unauthorized();
        }

        // GET api/users
        [Route("users")]
        public List<Dictionary<String, object>> GetUsers()
        {
            List<OnlineUser> users = MySQLHelper.getOnlineUsers(connection);
            List<Dictionary<String, object>> results = new List<Dictionary<string, object>>();

            foreach (var user in users)
            {
                Dictionary<string, object> result = new Dictionary<string, object>
            {
                { "login", user.login },
                { "ipAddress", user.ipAddress},
            };
                results.Add(result);
            }

            return results;
        }


        //DELETE api/user
        [Route("user/{login}")]
        [AllowAnonymous]
        public IHttpActionResult deleteUser(string login)
        {
            if (MySQLHelper.deleteUser(login, connection))
                return Ok();
            else
                return Unauthorized();
        }

        // POST api/logout
        [Route("logout")]
        [AllowAnonymous]
        public IHttpActionResult Logout(User user)
        {
            if (MySQLHelper.updateStatus(false, user.login, connection))
                return Ok();
            else
                return Unauthorized();
        }


        //POST api/login
        [Route("login")]
        [AllowAnonymous]
        public IHttpActionResult login(User user)
        {
            if (MySQLHelper.checkCorrectAccount(user.login, user.password, connection) && MySQLHelper.updateIPandStatus(user, connection))  
                return Ok();         
            else
                return Unauthorized();
        }
    }

}