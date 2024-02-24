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
                var res = await _httpClient.PostAsJsonAsync("http://26.90.89.142:5047/api/user/user", user);
                return await ServerResponse<string>.Create(res);
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
