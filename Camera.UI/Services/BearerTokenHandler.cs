using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Camera.UI.Services
{

    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _authorization;

        public BearerTokenHandler(IAccessTokenProvider authorization)
        {
            _authorization = authorization;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Получаем токен от провайдера
            string accessToken = _authorization.GetAccessToken();

            // Добавляем заголовок Authorization к каждому запросу
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private string _accessToken = string.Empty;

        public string GetAccessToken() => _accessToken;

        public void SetAccessToken(string accessToken) => _accessToken = accessToken;
        
    }
    public interface IAccessTokenProvider
    {
        void SetAccessToken(string accessToken);
        string GetAccessToken();
    }

}
