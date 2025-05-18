using System.Text;
using Newtonsoft.Json;
using westpac.Interfaces;

namespace westpac.Rest
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;

        public RestClient()
        {
            _httpClient = new HttpClient();
        }

        /**
         * Send a post request to given URL
         * @Url - Post URL
         * @Body - Content of the posting body.
         */
        public async Task<object?> PostRequest(string url, string body)
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

