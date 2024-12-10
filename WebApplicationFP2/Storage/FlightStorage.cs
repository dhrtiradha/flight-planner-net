using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplicationFP2.Database;
using WebApplicationFP2.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplicationFP2.Storage
{
    public class FlightStorage
    {
        private readonly FlightPlannerDbContext _context;

        public FlightStorage(FlightPlannerDbContext context)
        {
            _context = context;
        }

        public Flight AddFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            _context.SaveChanges();

            return flight;
        }

        public void ClearFlights()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.Airports.RemoveRange(_context.Airports);
            _context.SaveChanges();
        }

        public bool DeleteFlight(int id)
        {
            var flight = _context.Flights.FirstOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return false; 
            }

            _context.Flights.Remove(flight); 
            _context.SaveChanges(); 

            return true;
        }

        public bool IsFlightUnique(Flight flight)
        {
            return !_context.Flights.Any(f =>
                f.From.AirportCode == flight.From.AirportCode &&
                f.To.AirportCode == flight.To.AirportCode &&
                f.Carrier == flight.Carrier &&
                f.DepartureTime == flight.DepartureTime &&
                f.ArrivalTime == flight.ArrivalTime
            );
        }

        public List<Airport> GetUniqueAirports()
        {
            return _context.Flights
                .SelectMany(f => new[] { f.From, f.To }) 
                .DistinctBy(a => a.AirportCode)         
                .ToList();
        }

        public Flight GetFlightById(int id)
        {
            return _context.Flights
                .Include(f => f.From) 
                .Include(f => f.To)   
                .FirstOrDefault(f => f.Id == id);
        }

        public List<Flight> GetAllFlights()
        {
            return _context.Flights.ToList();
        }

        public List<Flight> GetFlightsByCriteria(string from, string to, DateTime departureDate)
        {
            var targetDate = departureDate.Date;

            return _context.Flights
                .Where(f =>
                    f.From.AirportCode.ToLower() == from.ToLower() &&
                    f.To.AirportCode.ToLower() == to.ToLower())
                .AsEnumerable() 
                .Where(f =>
                    DateTime.TryParse(f.DepartureTime, out var flightDepartureTime) &&
                    flightDepartureTime.Date == targetDate)
                .ToList();
        }

        public void AddAirport(Airport airport)
        {
            if (airport != null && !string.IsNullOrEmpty(airport.AirportCode))
            {
                _context.Add(airport);
            }
        }

        public IEnumerable<Airport> SearchAirports(string search)
        {
            var trimmedSearch = search.Trim().ToLower(); 

            return _context.Airports
                .Where(a => a.AirportCode.ToLower().Contains(trimmedSearch) ||
                            a.City.ToLower().Contains(trimmedSearch) ||
                            a.Country.ToLower().Contains(trimmedSearch))
                .ToList();
        }
    }
}
