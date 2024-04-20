using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Services.Airplane;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Management.System.API.Controllers.Airplane
{
    [Authorize(Policy = "AdminPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class AirplaneTypeController : Controller
    {
        private readonly AirplaneTypeService airplaneTypeService;

        public AirplaneTypeController(AirplaneTypeService airplaneTypeService)
        {
            this.airplaneTypeService = airplaneTypeService;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAirplaneById(int id)
        {
            var airplane = await airplaneTypeService.GetAirplaneType(id);

            if (airplane == null)
            {
                return NotFound();
            }

            return Ok(airplane);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateAirplaneType([FromBody] AirplaneTypeFormModel airplaneTypeFormModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await airplaneTypeService.CreateAsync(airplaneTypeFormModel);
            if (result != null)
            {
                return CreatedAtAction(nameof(GetAirplaneById), new { id = result.Id }, result);
            }
            else
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
