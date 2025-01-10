using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Runtime.InteropServices.JavaScript;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;


namespace WebApplicationFP2.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController(IEntityService<Flight> flightService) : ControllerBase
    {
        private readonly IEntityService<Flight> _flightService = flightService;

        //[HttpGet]
        //[Route("airports")]
        //public IActionResult SearchAirports(string search)
        //{
        //    if (string.IsNullOrWhiteSpace(search))
        //    {
        //        return BadRequest("Search query cannot be empty.");
        //    }

        //    var matchingAirports = _flightService.SearchAirports(search);

        //    if (!matchingAirports.Any())
        //    {
        //        return Ok(new List<Airport>());
        //    }

        //    return Ok(matchingAirports);
        //}

        
        //[HttpPost]
        //[Route("flights/search")]
        //public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        //{
        //    if (request == null || string.IsNullOrWhiteSpace(request.From) || string.IsNullOrWhiteSpace(request.To))
        //    {
        //        return BadRequest("Invalid request.");
        //    }

        //    if (request.From.Equals(request.To, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return BadRequest("Airports cannot be the same.");
        //    }

        //    if (!DateTime.TryParse(request.DepartureDate, out DateTime parsedDate))
        //    {
        //        return BadRequest("Invalid date format.");
        //    }

        //    var flights = _flightService.GetFlightsByCriteria(request.From, request.To, parsedDate);

        //    if (!flights.Any())
        //    {
        //        return Ok(new PageResult<Flight>
        //        {
        //            Page = 0,
        //            TotalItems = 0,
        //            Items = new List<Flight>()
        //        });
        //    }

        //    return Ok(new PageResult<Flight>
        //    {
        //        Page = 0,
        //        TotalItems = flights.Count,
        //        Items = flights
        //    });
        //}

        [HttpGet]
        [Route("flights/{id}")]
        public IActionResult GetFlightById(int id)
        {
            var flight = _flightService.GetById(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}