using Microsoft.AspNetCore.Components.Web;
using WebApplicationFP2.Models;

namespace WebApplicationFP2.Storage
{
    public static class FlightStorage
    {
        private static List<Flight> _flights = new List<Flight>();
        private static int _id = 0;

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = ++_id;
            _flights.Add(flight);

            return flight;
        }

        public static void ClearFlights()
        {
            _flights.Clear();
        }

        public static void DeleteFlight(int id)
        {
            _flights.RemoveAll(f => f.Id == id); 
        }

        public static bool FlightExists(Flight flight)
        {
            return _flights.Any(f =>
                f.From.AirportCode == flight.From.AirportCode &&
                f.To.AirportCode == flight.To.AirportCode &&
                f.Carrier == flight.Carrier &&
                f.DepartureTime == flight.DepartureTime &&
                f.ArrivalTime == flight.ArrivalTime);
        }

        public static List<Airport> GetUniqueAirports()
        {
            return _flights
                .SelectMany(f => new[] { f.From, f.To })
                .DistinctBy(a => a.AirportCode)         
                .ToList();
        }

        public static Flight GetFlightById(int id)
        {
            return _flights.FirstOrDefault(f => f.Id == id);  
        }

        public static List<Flight> GetAllFlights()
        {
            return _flights;
        }

        public static List<Flight> GetFlightsByCriteria(string from, string to, DateTime departureDate)
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
