using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Camera.UI.Models;
using Microsoft.Extensions.Configuration;

namespace Camera.UI.Services
{
    public interface IAuthorizationService
    {
        Task<ServerResponse<string>> AuthorizationAsync(User user);

        void SetToken(string token);
        
        string Token { get; }
    }
    
    public class AuthorizationService : IAuthorizationService
    {
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _accessTokenProvider;
        public AuthorizationService(HttpClient httpClient, IAccessTokenProvider accessTokenProvider)
        {
            _httpClient = httpClient;
            _accessTokenProvider = accessTokenProvider;
        }

        public void SetToken(string token)
        {
            _accessTokenProvider.SetAccessToken(token);
            _token = token;
        }

        public string Token => _token;
        private string _token;

        public async Task<ServerResponse<string>> AuthorizationAsync(User user)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("api/auth", user);

                var token = await ServerResponse<string>.CreateFromString(res);

                if (token.IsSuccess) 
                {
                    var parsedToken = ServerResponse<string>.ParseToken(token.Data);
                    _token = parsedToken.access_token;
                    SetToken(_token);
                }
                return token;
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
