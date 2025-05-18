namespace westpac.Interfaces
{
    public interface IRestClient
    {
        public Task<object?> PostRequest(string url, string body, bool isAuthorizationRequired = false);
    }
}

