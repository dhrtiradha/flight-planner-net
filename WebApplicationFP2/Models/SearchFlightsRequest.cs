using System.Text.Json.Serialization;
namespace WebApplicationFP2.Models
{
    public class SearchFlightsRequest
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("departureDate")]
        public string DepartureDate { get; set; }

        public SearchFlightsRequest(string from, string to, string departureDate)
        {
            From = from;
            To = to;
            DepartureDate = departureDate;
        }

        public SearchFlightsRequest() { }
    }
}
