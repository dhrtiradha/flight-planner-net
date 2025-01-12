using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationFP2.Models;

namespace WebApplicationFP2.Controllers
{
    [Route("admin-api")]
    [ApiController]
    [Authorize]
    public class AdminController(IEntityService<Flight> flightService) : ControllerBase
    {
        private static readonly object _lock = new object();
        private readonly IEntityService<Flight> _flightService = flightService;

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var result = _flightService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            var response = GetFromFlight(result);
            return Ok(result);
        }

        [HttpPost]
        [Route("flights")]
        public IActionResult AddFlight(FlightRequest request)
        {
            lock (_lock)
            {
                var flight = GetFromRequest(request);
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

                if (!_flightService.IsFlightUnique(flight))
                {
                    return Conflict("Flight already exists.");
                }

                var result = _flightService.Create(flight);
                var response = GetFromFlight(flight);
                response.Id = result.Entity.Id;

                return Created("", response);
            }
        }

        [HttpDelete]
        [Route("flights/{id}")]
        public IActionResult DeleteFlight(int id)
        {
            var flight = _flightService.GetById(id);

            if (flight == null)
            {
                return Ok();
            }

            var result = _flightService.Delete(flight);

            if (result == null)
            {
                return NotFound(); 
            }

            return Ok();
        }

        private Flight GetFromRequest(FlightRequest request)
        {
            return new Flight
            {
                ArrivalTime = request.ArrivalTime,
                Carrier = request.Carrier,
                DepartureTime = request.DepartureTime,
                From = new Airport
                {
                    AirportCode = request.From.Airport,
                    City = request.From.City,
                    Country = request.From.Country,
                },
                To = new Airport
                {
                    AirportCode = request.To.Airport,
                    City = request.To.City,
                    Country = request.To.Country,
                },
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
                From = new AirportResponse
                {
                    Airport = flight.From.AirportCode,
                    City = flight.From.City,
                    Country = flight.From.Country,
                },
                To = new AirportResponse
                {
                    Airport = flight.To.AirportCode,
                    City = flight.To.City,
                    Country = flight.To.Country,
                },
            };
        }
    }
}