using Microsoft.AspNetCore.Mvc;
using WebApplicationFP2.Storage;
using WebApplicationFP2.Models;
using System.Linq;
using System;
using System.Runtime.InteropServices.JavaScript;


namespace WebApplicationFP2.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly FlightStorage _storage;

        public CustomerController(FlightStorage storage)
        {
            _storage = storage;
        }

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var matchingAirports = AirportStorage.SearchThroughAirports(search);

            if (!matchingAirports.Any())
            {
                return Ok(new List<Airport>());
            }

            return Ok(matchingAirports);
        }

        
        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.From) || string.IsNullOrWhiteSpace(request.To))
            {
                return BadRequest("Invalid request.");
            }

            if (request.From.Equals(request.To, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Airports cannot be the same.");
            }

            if (!DateTime.TryParse(request.DepartureDate, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format.");
            }

            var flights = _storage.GetFlightsByCriteria(request.From, request.To, parsedDate);

            if (!flights.Any())
            {
                return Ok(new PageResult<Flight>
                {
                    Page = 0,
                    TotalItems = 0,
                    Items = new List<Flight>()
                });
            }

            return Ok(new PageResult<Flight>
            {
                Page = 0,
                TotalItems = flights.Count,
                Items = flights
            });
        }

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlightById(int id)
        {
            var flight = _storage.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}