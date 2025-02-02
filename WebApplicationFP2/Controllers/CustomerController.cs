using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Runtime.InteropServices.JavaScript;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using WebApplicationFP2.Models;


namespace WebApplicationFP2.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerController(IEntityService<Flight> flightService) : ControllerBase
    {
        private readonly IEntityService<Flight> _flightService = flightService;

        [HttpGet]
        [Route("airports")]
        public IActionResult SearchAirports(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var matchingAirports = _flightService.SearchAirports(search);

            if (!matchingAirports.Any())
            {
                return Ok(new List<Airport>());
            }

            else if (matchingAirports.Any())
            {
                var response = matchingAirports.Select(a => GetFromAirport(a)).ToList();
                return Ok(response);
            }

            return Ok(matchingAirports);
        }


        [HttpPost]
        [Route("flights/search")]
        public IActionResult SearchFlights([FromBody] SearchFlightRequest request)
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

            var flights = _flightService.GetFlightsByCriteria(request.From, request.To, parsedDate).ToList();

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
            Console.WriteLine($"Searching for flight with ID: {id}");

            var flight = _flightService.GetById(id);

            if (flight == null)
            {
                Console.WriteLine($"Flight with ID {id} not found.");
                return NotFound("Flight not found.");
            }

            var response = GetFromFlight(flight);
            return Ok(response);
        }

        private AirportResponse GetFromAirport(Airport airport)
        {
            return new AirportResponse
            {
                Airport = airport.AirportCode,  
                City = airport.City,
                Country = airport.Country
            };
        }

        private FlightResponse GetFromFlight(Flight flight)
        {
            return new FlightResponse
            {
                Id = flight.Id,
                ArrivalTime = flight.ArrivalTime,
                Carrier = flight.Carrier,
                DepartureTime = flight.DepartureTime,
                From = flight.From != null
                    ? new AirportResponse
                    {
                        Airport = flight.From.AirportCode ?? "",
                        City = flight.From.City ?? "",
                        Country = flight.From.Country ?? "",
                    }
                    : null, 

                To = flight.To != null
                    ? new AirportResponse
                    {
                        Airport = flight.To.AirportCode ?? "",
                        City = flight.To.City ?? "",
                        Country = flight.To.Country ?? "",
                    }
                    : null 
            };
        }
    }
}