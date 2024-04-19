using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Services.Airplane;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Management.System.API.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class AirplaneController : Controller
    {
        private readonly AirplaneService airplaneService;

        public AirplaneController(AirplaneService airplaneService)
        {
            this.airplaneService = airplaneService;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        /// <summary>
        /// Creates a new airplane entity with the provided airplane model data.
        /// </summary>
        /// <param name="airplaneModel">The model containing data for the new airplane.</param>
        /// <returns>
        /// If the airplane creation is successful, an HTTP response with status code 201 (Created) is returned, along with the created airplane entity.
        /// If the airplane creation fails due to invalid input data, an HTTP response with status code 400 (Bad Request) is returned, along with validation errors.
        /// If the airplane creation fails due to an internal server error, an HTTP response with status code 500 (Internal Server Error) is returned, along with an error message.
        /// </returns>

        public async Task<IActionResult> CreateAirplane([FromBody] AirplaneModel airplaneModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await this.airplaneService.CreateAsync(airplaneModel);

            if (result != null)
            {
                return CreatedAtAction(nameof(GetAirplaneById), new { id = result.Id }, result);
            }
            else
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("{id}")]
        /// <summary>
        /// Retrieves an airplane entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the airplane entity to retrieve.</param>
        /// <returns>
        /// If the airplane with the specified ID is found, an HTTP response with status code 200 (OK) is returned, along with the airplane entity.
        /// If no airplane with the specified ID is found, an HTTP response with status code 404 (Not Found) is returned.
        /// </returns>
        public async Task<IActionResult> GetAirplaneById(int id)
        {
            var airplane = await this.airplaneService.GetAirplane(id);

            if (airplane == null)
            {
                return NotFound();
            }

            return Ok(airplane);
        }
    }
}
