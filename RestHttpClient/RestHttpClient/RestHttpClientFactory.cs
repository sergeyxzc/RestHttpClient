using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RestHttpClient
{
    public static class RestHttpClientFactory
    {
        public static RestHttpClient Create(Uri baseAddress, IRestHttpSerializer serializer, IAuthHttpStrategy authHttpStrategy, TimeSpan? timeout = null)
        {
            var handler = new HttpClientHandler();

            if (authHttpStrategy != null && authHttpStrategy.UseDefaultCredentials())
                handler.UseDefaultCredentials = true;

            return Create(handler, baseAddress, authHttpStrategy, serializer, timeout);
        }

        public static RestHttpClient Create(HttpMessageHandler handler, Uri baseAddress, IAuthHttpStrategy authHttpStrategy, IRestHttpSerializer serializer, TimeSpan? timeout = null)
        {
            var client = new HttpClient(handler, true)
            {
                BaseAddress = baseAddress
            };

            if (timeout.HasValue)
                client.Timeout = timeout.Value;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return new RestHttpClient(client, authHttpStrategy, serializer);
        }
    }
}