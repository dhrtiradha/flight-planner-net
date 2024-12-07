using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using WebApplicationFP2.Database;
using WebApplicationFP2.Models;

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

        public void DeleteFlight(int id)
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.SaveChanges();
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
            return _context.Flights.FirstOrDefault(f => f.Id == id);
        }

        public List<Flight> GetAllFlights()
        {
            return _context.Flights.ToList();
        }

        public List<Flight> GetFlightsByCriteria(string from, string to, DateTime departureDate)
        {
            return _context.Flights
                .Where(f =>
                    f.From.AirportCode.Equals(from, StringComparison.OrdinalIgnoreCase) &&
                    f.To.AirportCode.Equals(to, StringComparison.OrdinalIgnoreCase))
                .AsEnumerable() 
                .Where(f =>
                    DateTime.TryParse(f.DepartureTime, out var flightDepartureTime) &&
                    flightDepartureTime.Date == departureDate.Date)
                .ToList();
        }
    }
}
