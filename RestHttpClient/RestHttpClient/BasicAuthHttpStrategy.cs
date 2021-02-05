using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestHttpClient
{
    public sealed class BasicAuthHttpStrategy : IAuthHttpStrategy
    {
        private readonly ICredentialsProvider _credentialsProvider;

        public BasicAuthHttpStrategy(ICredentialsProvider credentialsProvider)
        {
            _credentialsProvider = credentialsProvider;
        }

        public bool UseDefaultCredentials()
        {
            return false;
        }

        public async ValueTask UpdateHttpRequestHeadersAsync(HttpClient httpClient)
        {
            var credentials = await _credentialsProvider.GetCredentialsAsync();

            var val = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credentials.Username}:{credentials.Password}"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", val);
        }
    }
}