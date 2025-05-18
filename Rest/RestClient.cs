using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using westpac.Interfaces;

namespace westpac.Rest
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;

        private readonly IMemoryCache _memoryCache;

        public RestClient(IMemoryCache memoryCache)
        {
            _httpClient = new HttpClient();
            _memoryCache = memoryCache;
        }

        /**
         * Send a post request to given URL
         * @Url - Post URL
         * @Body - Content of the posting body.
         * @isAuthorizationRequired - This will ensure 
         *          to attach JWT token taken from memory cache to the request header
         */
        public async Task<object?> PostRequest(string url, string body, bool isAuthorizationRequired)
        {
            // Validating the URL
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));
            }

            // Validating the message body
            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException("Body cannot be null or empty.", nameof(body));
            }

            _httpClient.BaseAddress = new Uri(url);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            // Retrieve the JWT Token from memory cache and attach to header if the
            // authorization is required
            if (isAuthorizationRequired)
            {
                string token = _memoryCache.Get("JwtToken")?.ToString() ?? string.Empty;
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            }

            var httpResponse = await _httpClient.SendAsync(httpRequestMessage);

            if (httpResponse.Content != null)
            {
                // Returning the content as an object. As per our requirement,
                // we can read the data using dynamic type.
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(jsonString))
                {
                    return JsonConvert.DeserializeObject<object>(jsonString);
                }
            }

            return null;
        }

        /**
         * Send a get request to given URL
         * @Url - Get URL
         * @isAuthorizationRequired - This will ensure 
         *          to attach JWT token taken from memory cache to the request header
         */
        public async Task<object?> GetRequest(string url, bool isAuthorizationRequired)
        {
            // Validating the URL
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));
            }

            _httpClient.BaseAddress = new Uri(url);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            
            // Retrieve the JWT Token from memory cache and attach to header if the
            // authorization is required
            if (isAuthorizationRequired)
            {
                string token = _memoryCache.Get("JwtToken")?.ToString() ?? string.Empty;
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            }

            var httpResponse = await _httpClient.SendAsync(httpRequestMessage);

            if (httpResponse.Content != null)
            {
                // Returning the content as an object. As per our requirement,
                // we can read the data using dynamic type.
                var jsonString = await httpResponse.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(jsonString))
                {
                    return JsonConvert.DeserializeObject<object>(jsonString);
                }
            }

            return null;
        }
    }
}

