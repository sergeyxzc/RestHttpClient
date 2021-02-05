using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestHttpClient
{
    public class RestHttpClient : IDisposable
    {
        private readonly IAuthHttpStrategy _authHttpStrategy;
        private readonly IRestHttpSerializer _serializer;
        private readonly HttpClient _client;
        private volatile bool _disposed;

        public RestHttpClient(HttpClient client, IAuthHttpStrategy authHttpStrategy, IRestHttpSerializer serializer)
        {
            _client = client;
            _authHttpStrategy = authHttpStrategy;
            _serializer = serializer;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            _client.Dispose();
        }

        public async ValueTask<HttpResponseMessage> GetAsync(string url)
        {
            CheckDisposed();
            await UpdateAuthAsync();
            var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async ValueTask<TResponse> GetAsync<TResponse>(string url)
        {
            CheckDisposed();
            await UpdateAuthAsync();
            var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            return await ReadResponseAsync<TResponse>(response);
        }

        public async ValueTask<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            CheckDisposed();
            return await InternalPostAsync(url, content);
        }

        public async ValueTask<TResponse> PostAsync<TResponse>(string url, HttpContent content)
        {
            CheckDisposed();
            var response = await InternalPostAsync(url, content);
            return await ReadResponseAsync<TResponse>(response);
        }

        public async ValueTask PostAsync<TContent>(string url, TContent content)
        {
            CheckDisposed();
            var serializedContent = GetSerializedContent(content);
            await InternalPostAsync(url, serializedContent);
        }

        public async ValueTask<TResponse> PostAsync<TResponse, TContent>(string url, TContent content)
        {
            CheckDisposed();
            var serializedContent = GetSerializedContent(content);
            var response = await InternalPostAsync(url, serializedContent);
            return await ReadResponseAsync<TResponse>(response);
        }

        public async ValueTask DeleteAsync(string url)
        {
            CheckDisposed();
            await UpdateAuthAsync();
            var response = await _client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

        public async ValueTask<TResponse> DeleteAsync<TResponse>(string url)
        {
            CheckDisposed();
            await UpdateAuthAsync();
            var response = await _client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return await ReadResponseAsync<TResponse>(response);
        }

        private async ValueTask<HttpResponseMessage> InternalPostAsync(string url, HttpContent content)
        {
            await UpdateAuthAsync();
            var response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return response;
        }

        private StringContent GetSerializedContent<TContent>(TContent content)
        {
            var serializedContent = _serializer.SerializeContent(content);
            var stringContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            return stringContent;
        }

        private async ValueTask<TResponse> ReadResponseAsync<TResponse>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            return _serializer.DeserializeContent<TResponse>(result);
        }

        private async ValueTask UpdateAuthAsync()
        {
            if (_authHttpStrategy == null)
                return;

            await _authHttpStrategy.UpdateHttpRequestHeadersAsync(_client);
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("RestHttpClient is disposed.");
        }
    }
}
