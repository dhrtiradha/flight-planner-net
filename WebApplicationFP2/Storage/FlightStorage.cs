using Microsoft.AspNetCore.Components.Web;
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
            _context.Flights.RemoveRange();
        }

        public void DeleteFlight(int id)
        {
            _flights.RemoveAll(f => f.Id == id); 
        }

        //public static bool FlightExists(Flight flight)
        //{
        //    return _flights.Any(f =>
        //        f.From.AirportCode == flight.From.AirportCode &&
        //        f.To.AirportCode == flight.To.AirportCode &&
        //        f.Carrier == flight.Carrier &&
        //        f.DepartureTime == flight.DepartureTime &&
        //        f.ArrivalTime == flight.ArrivalTime);
        //}

        public List<Airport> GetUniqueAirports()
        {
            return _flights
                .SelectMany(f => new[] { f.From, f.To })
                .DistinctBy(a => a.AirportCode)         
                .ToList();
        }

        public Flight GetFlightById(int id)
        {
            return _flights.FirstOrDefault(f => f.Id == id);  
        }

        public List<Flight> GetAllFlights()
        {
            return _flights;
        }

        public List<Flight> GetFlightsByCriteria(string from, string to, DateTime departureDate)
        {
            return _flights
                .Where(f =>
                    f.From.AirportCode.Equals(from, StringComparison.OrdinalIgnoreCase) &&
                    f.To.AirportCode.Equals(to, StringComparison.OrdinalIgnoreCase) &&
                    DateTime.TryParse(f.DepartureTime, out var flightDepartureTime) &&
                    flightDepartureTime.Date == departureDate.Date)  
                .ToList();
        }

    }
}
