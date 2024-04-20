
using Flight.Management.System.API.Models.Flight;
using Flight.Management.System.API.Services.Flight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Management.System.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
  
    public class FlightController : Controller
    {
        private readonly FlightService flightService;

        public FlightController(FlightService flightService)
        {
            this.flightService = flightService;
        }
        #region get functions
        [HttpGet]
        /// <summary>
        /// Retrieves all flights available in the system.
        /// </summary>
        /// <returns>
        /// Returns an HTTP response with status code 200 (OK) along with the list of flights if available.
        /// If there ar
        public async Task<IActionResult> GetAllFlight()
        {
            List<Flight.Management.System.Data.Model.Flight> listOfFlight = await this.flightService.GetAllFlights();
            if (listOfFlight.Count == 0)
                return NoContent();
            return Ok(listOfFlight);
        }
      
        [HttpGet("id")]
        /// <summary>
        /// Retrieves flight information by its numerical ID.
        /// </summary>
        /// <param name="id">The numerical identifier of the flight to be retrieved.</param>
        /// <returns>
        /// Returns an HTTP response with status code 200 (OK) along with the flight object if found.
        /// Returns an HTTP response with status code 204 (No Content) if the flight is not found.
        /// </returns>
        public async Task<IActionResult> GetById(int id)
        {
            Flight.Management.System.Data.Model.Flight Flight = await this.flightService.GetFlight(id);
            if(Flight== null)
            {
                return NoContent();
            }
            return Ok(Flight);
        }   
        
        [HttpGet("publicId")]
        /// <summary>
        /// Retrieves flight information by its public ID.
        /// </summary>
        /// <param name="id">The public identifier of the flight to be retrieved.</param>
        /// <returns>
        /// Returns an HTTP response with status code 200 (OK) along with the flight object if found.
        /// If the flight is not found, the response body will be empty.
        /// </returns>
        public async Task<IActionResult> GetById(string id)
        {
            Flight.Management.System.Data.Model.Flight Flight = this.flightService.GetFlight(id);
            return Ok(Flight);
        }
        #endregion

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        /// <summary>
        /// Handles requests to create a new flight.
        /// </summary>
        /// <param name="flightModel">The model representing the data of the new flight.</param>
        /// <returns>Returns a reference to the newly created resource (flight).</returns>
        /// <remarks>Accessible only to users authorized with the "AdminPolicy" policy.</remarks>
        public async Task<IActionResult> CreateFlight([FromBody] FlightModel flightModel)
        {
            if(!ModelState.IsValid)
            {                
                return BadRequest(ModelState);
            }
            var entity = await flightService.CreateAsync(flightModel);
            return CreatedAtAction($"~/users/{entity.Id}", entity);
           
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight(int id, [FromBody] FlightModel flightModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await flightService.Update(id, flightModel);
            if(entity!=null)
            {
                return NoContent();

            }
            return NotFound();


        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("id")]
        /// <summary>
        /// Deletes a flight by its ID.
        /// </summary>
        /// <param name="id">The identifier of the flight to be deleted.</param>
        /// <returns>Returns an HTTP response with status code 200 (OK) upon successful deletion.</returns>
        /// <remarks>Accessible only to users authorized with the "AdminPolicy" policy.</remarks>
        public async Task<IActionResult> DeleteById(int id)
        {
          
            this.flightService.DeleteById(id);
            return Ok();
        }
    }
}
