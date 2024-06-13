using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Camera.UI.Models;

namespace Camera.UI.Services
{
    public interface IRegistrationService
    {
        public Task<ServerResponse<string>> Registration(User user);
        public Task<ServerResponse<User>> GetMe();
        Task<ServerResponse<string>> ChangeUser(User user);
    }
    
    public class RegistrationService : IRegistrationService
    {
        private readonly HttpClient _httpClient;
        
        public RegistrationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<ServerResponse<string>> Registration(User user)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("api/user/user", user);
                return await ServerResponse<string>.CreateFromString(res);
            }
            catch (Exception e)
            {
                return new ServerResponse<string>()
                {
                    IsSuccess = false,
                    Error = e.Message
                };
            }
        }

        public async Task<ServerResponse<User>> GetMe()
        {
            try
            {
                var res = await _httpClient.GetAsync("api/user/currentuser");
                return await ServerResponse<Joint.Data.Models.User>.CreateFromJson(res);
            }
            catch (Exception e)
            {
                return new ServerResponse<User>()
                {
                    IsSuccess = false,
                    Error = e.Message
                };
            }
        }
        public async Task<ServerResponse<string>> ChangeUser(User user)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("api/user/changeuser", user);
                return await ServerResponse<string>.CreateFromString(res);
            }
            catch (Exception e)
            {
                return new ServerResponse<string>()
                {
                    IsSuccess = false,
                    Error = e.Message
                };
            }
        }
    }
}
