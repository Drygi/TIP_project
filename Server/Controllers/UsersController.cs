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
        // GET api/users
        [Route("users")]
        public List<Dictionary<String, object>> GetUsers()
        {
            UserManager users = new UserManager();
            List<Dictionary<String, object>> results = new List<Dictionary<string, object>>();
          foreach (User user in users.users)
            {
                Dictionary<string, object> result = new Dictionary<string, object>
            {
                { "login", user.login },
                { "ipAddress", user.ipAddress},
                { "status", user.Status }
            };
                results.Add(result);
            }

            return results;
        }

        // POST api/register
        [AllowAnonymous]
        [Route("register")]     
        public IHttpActionResult Register(RegisterUser userModel)
        {
            var responseDict = new Dictionary<string, RegisterUser>();
            HttpResponseMessage responseMsg = Request.CreateResponse
                (HttpStatusCode.OK, responseDict, new MediaTypeHeaderValue("application/json"));    
            IHttpActionResult response = ResponseMessage(responseMsg);
            MySQLHelper.insertUser(userModel, connection);
            
            return response;
           
        }
    }

}