using System;
namespace westpac.Interfaces
{
	public interface IRestClient
	{
		public Task<HttpResponseMessage> PostRequest(string url, string body);
	}
}

