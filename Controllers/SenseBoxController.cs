using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using westpac.Interfaces;
using westpac.Models;
using westpac.Resolvers;

namespace westpac.Controllers;

[ApiController]
[Route("[controller]")]
public class SenseBoxController : ControllerBase
{
    private readonly IRestClient _restClient;

    public SenseBoxController(IRestClient restClient)
    {
        _restClient = restClient;
    }

    [HttpPost("NewSenseBox")]
    /**
     * User login
     */
    public async Task<object?> NewSenseBox([FromBody] SenseBox senseBox)
    {
        // Convert the user object to JSON
        var jsonBody = LowercaseJsonSerializer.SerializeObject(senseBox);

        // Send the POST request
        var response = await _restClient.PostRequest("https://api.opensensemap.org/boxes", jsonBody, true);

        // Return the response as a string
        // NOTE 1: We might want to handle the response differently based on our requirements
        // For example, we could deserialize it into a specific object type
        // or check for success/failure and return appropriate messages.
        // For now, we'll just return the raw response as a string
        // NOTE 2: Current requirement is to return the response from opensensemap as it is.
        try
        {
            var code = (response as dynamic).code;
            if (!string.IsNullOrEmpty(code))
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
