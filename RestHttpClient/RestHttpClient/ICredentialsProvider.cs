using System.Threading.Tasks;

namespace RestHttpClient
{
    public interface ICredentialsProvider
    {
        ValueTask<(string Username, string Password)> GetCredentialsAsync();
    }
}