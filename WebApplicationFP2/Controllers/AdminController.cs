using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationFP2.Database;
using WebApplicationFP2.Models;
using WebApplicationFP2.Storage;

namespace WebApplicationFP2.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private static readonly object _lock = new object();
        private readonly FlightStorage _storage;

        public AdminController(FlightStorage storage)
        {
            _storage = storage;
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            return NotFound();
        }

        [HttpPost]
        [Route("flights")]
        public IActionResult AddFlight(Flight flight)
        {
            lock (_lock)
            {
                if (flight.From == null || flight.To == null || string.IsNullOrEmpty(flight.Carrier) ||
                string.IsNullOrEmpty(flight.DepartureTime) || string.IsNullOrEmpty(flight.ArrivalTime) ||
                string.IsNullOrEmpty(flight.From.AirportCode) || string.IsNullOrEmpty(flight.To.AirportCode)) 
                { 
                    return BadRequest("Invalid flight data.");
                }
                
                if (string.Equals(flight.From.AirportCode.Trim(), flight.To.AirportCode.Trim(), StringComparison.OrdinalIgnoreCase)) 
                { 
                    return BadRequest("Departure and destination airports cannot be the same.");
                }
                
                if (DateTime.TryParse(flight.DepartureTime, out var departureTime) &&
                    DateTime.TryParse(flight.ArrivalTime, out var arrivalTime)) 
                { 
                    if (arrivalTime <= departureTime) 
                    { 
                        return BadRequest("Arrival time must be after departure time.");
                    }
                }
                else 
                { 
                    return BadRequest("Invalid date format.");
                }

                if (!_storage.IsFlightUnique(flight))
                {
                    return Conflict("Flight already exists.");
                }

                _storage.AddAirport(flight.From); 
                _storage.AddAirport(flight.To);

                _storage.AddFlight(flight);

                return Created("", flight);
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            _storage.DeleteFlight(id);

            return Ok();
        }
    }
}