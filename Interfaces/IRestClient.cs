namespace westpac.Interfaces
{
    public interface IRestClient
    {
        public Task<object?> PostRequest(string url, string body, bool isAuthorizationRequired = false);

        public Task<object?> GetRequest(string url, bool isAuthorizationRequired = false);
    }
}

