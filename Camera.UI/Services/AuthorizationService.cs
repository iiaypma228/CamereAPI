using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Camera.UI.Services
{
    internal class AuthorizationService
    {
        public bool Authorization(User user)
        {
            var http = new HttpClient();
            var res = http.PostAsJsonAsync("http://26.90.89.142:5047/api/auth", user).Result;
            var text = res.Content.ReadAsStringAsync().Result;
            return true;
        }
    }
}
