using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Camera.UI.Services
{

    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IAuthorizationService _authorization;

        public BearerTokenHandler(IAuthorizationService authorization)
        {
            _authorization = authorization;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Получаем токен от провайдера
            string accessToken = _authorization.Token;

            // Добавляем заголовок Authorization к каждому запросу
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private string _accessToken;
        private IAuthorizationService _authorizationService;

        public AccessTokenProvider(IAuthorizationService service)
        {
            _authorizationService = service;
        }

        public async Task<string> GetAccessToken()
        {
            // Вернуть текущий токен или запросить новый токен
            // Возможно, вам потребуется логика обновления токена
            return _authorizationService.Token;
        }

        public void SetAccessToken(string accessToken)
        {
            // Установить токен после успешной аутентификации
            _accessToken = accessToken;
        }
    }
    public interface IAccessTokenProvider
    {
        Task<string> GetAccessToken();
    }

}
