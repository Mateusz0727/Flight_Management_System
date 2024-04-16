using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Models.Flight;
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
        public async Task<IActionResult> CreateAirplane([FromBody] AirplaneModel airplaneModel)
        {
            var entity = this.airplaneService.CreateAsync(airplaneModel);
            return Created($"~/users/{entity.Id}", entity); ;
        }
    }
}
