using Microsoft.AspNetCore.Mvc;
using westpac.Interfaces;

namespace westpac.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterUserController : ControllerBase
{
    private readonly ILogger<RegisterUserController> _logger;

    private readonly IRestClient _restClient;

    public RegisterUserController(ILogger<RegisterUserController> logger, IRestClient restClient)
    {
        _restClient = restClient;
        _logger = logger;
    }

    [HttpPost(Name = "RegisterUser")]
    public Task<HttpResponseMessage> Post(string name, string email, string password, string language)
    {
        var user = new User
        {
            Name = name,
            Email = email,
            Password = password,
            Language = language
        };

        // Convert the user object to JSON
        var jsonBody = System.Text.Json.JsonSerializer.Serialize(user);

        // Send the POST request
        var response = _restClient.PostRequest("https://api.opensensemap.org/users/register", jsonBody);

        return response;
    }
}
