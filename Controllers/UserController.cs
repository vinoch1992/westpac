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
    /**
     * Registering a new user
     */
    public async Task<object?> RegisterUser([FromBody] User user)
    {
        // Validating the input name
        var isValidName = StringValidators.IsValidName(user.Name);
        if (!isValidName.Item1)
        {
            return RestResponse(400, "ValidationError", isValidName.Item2);
        }

        // Validating the input email
        var isValidEmail = StringValidators.IsValidEmail(user.Email);
        if (!isValidEmail.Item1)
        {
            return RestResponse(400, "ValidationError", isValidEmail.Item2);
        }

        // Validating the input password
        var isValidPassword = StringValidators.IsValidPassword(user.Password);
        if (!isValidPassword.Item1)
        {
            return RestResponse(400, "ValidationError", isValidPassword.Item2);
        }

        // Setting the default language if that is not present in the input.
        user.Language = string.IsNullOrEmpty(user.Language) ? "en_US" : user.Language;

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
    /**
     * User login
     */
    public async Task<object?> Login([FromBody] User user)
    {
        // Validating the input email
        var isValidEmail = StringValidators.IsValidEmail(user.Email);
        if (!isValidEmail.Item1)
        {
            return RestResponse(400, "ValidationError", isValidEmail.Item2);
        }

        // Validating the input password
        var isValidPassword = StringValidators.IsValidPassword(user.Password);
        if (!isValidPassword.Item1)
        {
            return RestResponse(400, "ValidationError", isValidPassword.Item2);
        }

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

    /**
     * Returns an object of GenericResponse to output to API.
     * @responsecode - request response code
     * @code - Message Title
     * @message - Message description
     */
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
