using System.Net.Http;
using System.Threading.Tasks;

namespace RestHttpClient
{
    public sealed class DefaultCredentialsAuthHttpStrategy : IAuthHttpStrategy
    {
        public bool UseDefaultCredentials()
        {
            return true;
        }

#pragma warning disable 1998
        public async ValueTask UpdateHttpRequestHeadersAsync(HttpClient httpClient)
#pragma warning restore 1998
        {
        }
    }
}