using Microsoft.AspNetCore.Mvc;
using westpac.Interfaces;
using westpac.Resolvers;

namespace westpac.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IRestClient _restClient;

    public UserController(IRestClient restClient)
    {
        _restClient = restClient;
    }

    [HttpPost("RegisterUser")]
    public async Task<string?> RegisterUser(string name, string email, string password, string language)
    {
        var user = new User
        {
            Name = name,
            Email = email,
            Password = password,
            Language = language
        };

        // Convert the user object to JSON
        var jsonBody = LowercaseJsonSerializer.SerializeObject(user);

        // Send the POST request
        var response = await _restClient.PostRequest("https://api.opensensemap.org/users/register", jsonBody);

        return response?.ToString();
    }

    [HttpPost("Login")]
    public async Task<string?> Login(string email, string password)
    {
        var user = new User
        {
            Email = email,
            Password = password
        };

        // Convert the user object to JSON
        var jsonBody = LowercaseJsonSerializer.SerializeObject(user);

        // Send the POST request
        var response = await _restClient.PostRequest("https://api.opensensemap.org/users/sign-in", jsonBody);

        return response?.ToString();
    }
}
