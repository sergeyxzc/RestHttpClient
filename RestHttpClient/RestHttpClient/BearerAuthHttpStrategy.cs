using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestHttpClient
{
    public sealed class BearerAuthHttpStrategy : IAuthHttpStrategy
    {
        private readonly ITokenProvider _tokenProvider;

        public BearerAuthHttpStrategy(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public bool UseDefaultCredentials()
        {
            return false;
        }

        public async ValueTask UpdateHttpRequestHeadersAsync(HttpClient httpClient)
        {
            if (_tokenProvider == null)
                return;

            var bearerToken = await _tokenProvider.GetTokenAsync();

            if (bearerToken != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }
        } 
    }
}