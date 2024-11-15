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

        public static bool FlightExists(Flight flight)
        {
            return _flights.Any(f =>
                f.From.AirportCode == flight.From.AirportCode &&
                f.To.AirportCode == flight.To.AirportCode &&
                f.Carrier == flight.Carrier &&
                f.DepartureTime == flight.DepartureTime &&
                f.ArrivalTime == flight.ArrivalTime);
        }
    }
}
