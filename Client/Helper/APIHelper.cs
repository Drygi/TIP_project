using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helper
{
    public static class APIHelper
    {
        public static async Task<bool> login(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://"+GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/login", content);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        public static async Task<bool> deleteUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
               
                // HTTP DELETE
                HttpResponseMessage response = await client.DeleteAsync("api/user/"+user.login);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        public static async Task<bool> register(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/user", content);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        public static async Task<List<OnlineUser>> getOnlineUsers()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://" + GlobalMemory.serverAddressIP+"/api/users/");

                response.EnsureSuccessStatusCode();

                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<OnlineUser>>(responseBody);

                }

            }
        }
        public static async Task<bool> logout(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/logout", content);

               if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
        public static async Task<bool> findLogin(OnlineUser user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/findLogin", content);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public static async Task<bool> isOnline(OnlineUser user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + GlobalMemory.serverAddressIP);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("api/isOnline", content);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

    }
}
