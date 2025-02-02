using FlightPlanner.Core.Models;

namespace WebApplicationFP2.Models
{
    public class FlightRequest
    {
        public AirportRequest From { get; set; }
        public AirportRequest To { get; set; }
        public string Carrier { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}
