using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UniversitiesAPI.Request;
using UniversitiesAPI.Response;

// author: feldy judah k
// .NET 8

namespace UniversitiesAPI.Controllers
{
    [ApiController]
    [Route("university")]
    public class UniversityController : ControllerBase
    {
        private readonly ILogger<UniversityController> _logger;

        public UniversityController(ILogger<UniversityController> logger)
        {
            _logger = logger;
        }

        [Produces("application/json")]
        [HttpGet("get_universities")]
        public async Task<IActionResult> GetUniversities([FromQuery] GetUniversitiesRequest request, CancellationToken cancellationToken)
        {
            // Create a new HttpClient instance
            HttpClient httpClient = new HttpClient();

            // Define the API endpoint
            string ApiURL = "http://universities.hipolabs.com/search";
            string param1Key = "name=";
            string param1Value = request.Name ?? ""; // optional
            string param2Key = "country=";
            string param2Value = request.Country;

            // Full URL with query string
            ApiURL = $"{ApiURL}?{param1Key}{param1Value}&{param2Key}{param2Value}";

            try
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "UniversitiesAPI");
                // if using auth header JWT:
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "your-token");

                // Send GET request
                var response = await httpClient.GetAsync(ApiURL, cancellationToken);

                // If POST then:
                // var response = await httpClient.PostAsync(ApiURL, content, cancellationToken);

                // Ensure the response status is successful
                response.EnsureSuccessStatusCode();

                // Read and display the response
                string responseJsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                var ListUniversities = JsonSerializer.Deserialize<List<GetUniversitiesResponse>>(responseJsonString)
                                        ?? new List<GetUniversitiesResponse>();

                // create parent json
                var GenResponse = new GenericResponse<GetUniversitiesResponse>()
                {
                    Middleware = "UniversitiesAPI",
                    Entity = ListUniversities
                };

                // dispose HttpClient
                httpClient.Dispose();

                return Ok(GenResponse);
            }
            catch (Exception ex)
            {
                // dispose HttpClient
                httpClient.Dispose();

                // Handle other exceptions
                return BadRequest(ex.Message);
            }
        }


    }
}
