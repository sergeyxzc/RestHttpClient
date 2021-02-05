using System.Net.Http;
using System.Threading.Tasks;

namespace RestHttpClient
{
    public interface IAuthHttpStrategy
    {
        bool UseDefaultCredentials();
        ValueTask UpdateHttpRequestHeadersAsync(HttpClient httpClient);
    }
}