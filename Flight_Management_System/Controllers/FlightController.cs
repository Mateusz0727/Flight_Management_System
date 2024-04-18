﻿
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
        public async Task<IActionResult> GetAllFlight()
        {
            List<Flight.Management.System.Data.Model.Flight> listOfFlight = await this.flightService.GetAllFlights();
            if (listOfFlight.Count == 0)
                return NoContent();
            return Ok(listOfFlight);
        }
      
        [HttpGet("id")]
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
        public async Task<IActionResult> GetById(string id)
        {
            Flight.Management.System.Data.Model.Flight Flight = this.flightService.GetFlight(id);
            return Ok(Flight);
        }
        #endregion

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
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
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteById(int id)
        {
          
            this.flightService.DeleteById(id);
            return Ok();
        }
    }
}
