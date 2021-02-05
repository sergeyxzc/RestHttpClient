using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RestHttpClient
{
    public static class RestHttpClientFactory
    {
        public static RestHttpClient Create(Uri baseAddress, IAuthHttpStrategy authHttpStrategy = null,
            TimeSpan? timeout = null, JsonSerializerOptions jsonSerializerOptions = null)
        {
            var handler = new HttpClientHandler();

            if (authHttpStrategy != null && authHttpStrategy.UseDefaultCredentials())
                handler.UseDefaultCredentials = authHttpStrategy.UseDefaultCredentials();

            return Create(handler, baseAddress, timeout, authHttpStrategy, jsonSerializerOptions);
        }

        public static RestHttpClient Create(HttpMessageHandler handler, Uri baseAddress, TimeSpan? timeout = null,
            IAuthHttpStrategy authHttpStrategy = null, JsonSerializerOptions jsonSerializerOptions = null)
        {
            var client = new HttpClient(handler, true)
            {
                BaseAddress = baseAddress
            };

            if (timeout.HasValue)
                client.Timeout = timeout.Value;

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return new RestHttpClient(client, authHttpStrategy, jsonSerializerOptions);
        }
    }
}