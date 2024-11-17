using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                
                var flights = FlightStorage.GetAllFlights(); 
                var matchingFlights = flights.Where(f =>
                f.From.AirportCode == flight.From.AirportCode &&
                f.To.AirportCode == flight.To.AirportCode &&
                f.Carrier == flight.Carrier &&
                f.DepartureTime == flight.DepartureTime &&
                f.ArrivalTime == flight.ArrivalTime
                ).ToList();
                
                if (matchingFlights.Any()) 
                { 
                    return Conflict("The flight already exists.");
                }
                else 
                { 
                    AirportStorage.AddAirport(flight.From); 
                    AirportStorage.AddAirport(flight.To);
                    
                    var addedFlight = FlightStorage.AddFlight(flight); 
                    return Created("", addedFlight);
                }
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {

            FlightStorage.DeleteFlight(id);
            return Ok();

        }
    }
}