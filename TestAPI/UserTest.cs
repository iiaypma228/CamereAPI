using Camera.Controllers;
using Joint.Data.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace TestAPI
{
    public class UserTest : BaseTest
    {

        [Fact]
        public void TestRegistration()
        {
            var res = _httpClient.PostAsJsonAsync($"api/User/user", _user).Result;

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
        [Fact]
        public void TestGetMe()
        {
            var res = _httpClient.GetAsync($"/api/User/currentuser").Result;
            var jsonStr = res.Content.ReadAsStringAsync().Result;
            var user = System.Text.Json.JsonSerializer.Deserialize<User>(jsonStr);
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal(_userExist.Email, user.Email);
        }
    }
}