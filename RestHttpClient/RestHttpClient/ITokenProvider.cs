using System.Threading.Tasks;

namespace RestHttpClient
{
    public interface ITokenProvider
    {
        void ResetToken();
        ValueTask<string> GetTokenAsync();
    }
}
