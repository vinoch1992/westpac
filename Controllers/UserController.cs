using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using westpac.Interfaces;
using westpac.Models;
using westpac.Resolvers;
using westpac.Validators;

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
    public async Task<object?> RegisterUser(string name, string email, string password, string? language)
    {
        // Validating the input name
        var isValidName = StringValidators.IsValidName(name);
        if (!isValidName.Item1)
        {
            return RestResponse(400, "ValidationError", isValidName.Item2);
        }

        // Validating the input email
        var isValidEmail = StringValidators.IsValidEmail(email);
        if (!isValidEmail.Item1)
        {
            return RestResponse(400, "ValidationError", isValidEmail.Item2);
        }

        // Validating the input password
        var isValidPassword = StringValidators.IsValidPassword(password);
        if (!isValidPassword.Item1)
        {
            return RestResponse(400, "ValidationError", isValidPassword.Item2);
        }

        var user = new User
        {
            Name = name,
            Email = email,
            Password = password,
            Language = language ?? "en_US"
        };

        // Convert the user object to JSON
        var jsonBody = LowercaseJsonSerializer.SerializeObject(user);

        // Send the POST request
        var response = await _restClient.PostRequest("https://api.opensensemap.org/users/register", jsonBody);

        // Return the response as a string
        // NOTE 1: We might want to handle the response differently based on our requirements
        // For example, we could deserialize it into a specific object type
        // or check for success/failure and return appropriate messages.
        // For now, we'll just return the raw response as a string
        // NOTE 2: Current requirement is to return the response from opensensemap as it is.
        try
        {
            var code = (response as dynamic).code;
            if (code.ToString() != "Created")
            {
                return RestResponse(400, code.ToString(), (response as dynamic).message.ToString());
            }

            return JsonConvert.DeserializeObject<ExpandoObject>(response.ToString());
        }
        catch (Exception)
        {
            return RestResponse(400, "Error", "Something went wrong");
        }
    }

    [HttpPost("Login")]
    public async Task<object?> Login(string email, string password)
    {
        // Validating the input email
        var isValidEmail = StringValidators.IsValidEmail(email);
        if (!isValidEmail.Item1)
        {
            return RestResponse(400, "ValidationError", isValidEmail.Item2);
        }

        // Validating the input password
        var isValidPassword = StringValidators.IsValidPassword(password);
        if (!isValidPassword.Item1)
        {
            return RestResponse(400, "ValidationError", isValidPassword.Item2);
        }

        var user = new User
        {
            Email = email,
            Password = password
        };

        // Convert the user object to JSON
        var jsonBody = LowercaseJsonSerializer.SerializeObject(user);

        // Send the POST request
        var response = await _restClient.PostRequest("https://api.opensensemap.org/users/sign-in", jsonBody);

        // Return the response as a string
        // NOTE 1: We might want to handle the response differently based on our requirements
        // For example, we could deserialize it into a specific object type
        // or check for success/failure and return appropriate messages.
        // For now, we'll just return the raw response as a string
        // NOTE 2: Current requirement is to return the response from opensensemap as it is.
        try
        {
            var code = (response as dynamic).code;
            if (code.ToString() != "Authorized")
            {
                return RestResponse(400, code.ToString(), (response as dynamic).message.ToString());
            }

            return JsonConvert.DeserializeObject<ExpandoObject>(response.ToString());
        }
        catch (Exception)
        {
            return RestResponse(400, "Error", "Something went wrong");
        }
    }

    private object RestResponse(int responsecode, string code, string message)
    {
        HttpContext.Response.StatusCode = responsecode;
        return new GenericResponse()
        {
            Code = code,
            Message = message
        };
    }
}
