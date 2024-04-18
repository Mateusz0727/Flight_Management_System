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
        public async Task<IActionResult> CreateAirplane([FromBody] AirplaneModel airplaneModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await this.airplaneService.CreateAsync(airplaneModel);
            return Created($"~/users/{entity.Id}", entity);
        }
    }
}
