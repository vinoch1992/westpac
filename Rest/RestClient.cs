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

        private readonly IConfiguration _configuration;

        public RestClient(HttpClient httpClient, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        /**
         * Send a post request to given URL
         * @Url - Post URL
         * @Body - Content of the posting body.
         * @isAuthorizationRequired - This will ensure 
         *          to attach JWT token taken from memory cache to the request header
         */
        public async Task<object?> PostRequest(string url, string? body, bool isAuthorizationRequired)
        {
            // Get base URL from app settings and concatenate
            url = _configuration["BaseURL"] + url;

            // Validating the URL
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));
            }

            // Add base address
            _httpClient.BaseAddress = new Uri(url);

            // Prepare the request message
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            // Attach request body if the body has value
            if (!string.IsNullOrEmpty(body))
            {
                httpRequestMessage.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            // Retrieve the JWT Token from memory cache and attach to header if the
            // authorization is required
            if (isAuthorizationRequired)
            {
                string token = _memoryCache.Get("JwtToken")?.ToString() ?? string.Empty;
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + token);
            }

            // Send request and wait for response
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
            // Get base URL from app settings and concatenate
            url = _configuration["BaseURL"] + url;

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

