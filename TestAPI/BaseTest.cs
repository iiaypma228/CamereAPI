using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestAPI
{

    public class BaseTest
    {
        protected readonly HttpClient _httpClient;

        public BaseTest()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
        }

        protected void GetToken(User user)
        {
            var res = _httpClient.PostAsJsonAsync($"api/auth", user).Result;

            // Десериализация в JsonDocument для извлечения значений
            using JsonDocument doc = JsonDocument.Parse(res.Content.ReadAsStringAsync().Result);
            JsonElement root = doc.RootElement;

            // Извлечение значения по ключу
            string token = root.GetProperty("access_token").GetString();

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        }

    }
}
