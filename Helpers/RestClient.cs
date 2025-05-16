using System;
using System.Text;
using westpac.Interfaces;
namespace westpac.Helpers
{
	public class RestClient : IRestClient
	{
		private readonly HttpClient _httpClient;

		public RestClient()
		{
			_httpClient = new HttpClient();
		}
		public async Task<HttpResponseMessage> PostRequest(string url, string body)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentException("URL cannot be null or empty.", nameof(url));
			}

			if (string.IsNullOrEmpty(body))
			{
				throw new ArgumentException("Body cannot be null or empty.", nameof(body));
			}

			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = new StringContent(body, Encoding.UTF8, "application/json")
			};

			return await _httpClient.SendAsync(httpRequestMessage);
		}
	}
}

