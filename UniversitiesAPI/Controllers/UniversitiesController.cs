using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UniversitiesAPI.Request;
using UniversitiesAPI.Response;

namespace UniversitiesAPI.Controllers
{
    [ApiController]
    [Route("universities")]
    public class UniversitiesController : ControllerBase
    {
        private readonly ILogger<UniversitiesController> _logger;

        public UniversitiesController(ILogger<UniversitiesController> logger)
        {
            _logger = logger;
        }

        [Produces("application/json")]
        [HttpGet("get_universities_by_country")]
        public async Task<IActionResult> GetUniversitiesByCountry([FromQuery]GetUniversitiesByCountryRequest request, CancellationToken cancellationToken)
        {
            // Create a new HttpClient instance
            using var httpClient = new HttpClient();

            // Define the API endpoint
            string ApiURL = "http://universities.hipolabs.com/search";
            string QueryStringKey = "?country=";
            string QueryStringValue = request.Country;

            // Full URL with query string
            ApiURL = $"{ApiURL}{QueryStringKey}{QueryStringValue}";

            try
            {
                // Send GET request
                var response = await httpClient.GetAsync(ApiURL, cancellationToken);

                // Ensure the response status is successful
                response.EnsureSuccessStatusCode();

                // Read and display the response
                string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                var Responses = JsonSerializer.Deserialize<List<GetUniversitiesByCountryResponse>>(responseBody);

                return Ok(Responses);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return BadRequest(ex.Message);
            }
        }


    }
}
