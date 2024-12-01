using System.Text.Json.Serialization;

namespace WebApplicationFP2.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        [JsonPropertyName("airport")] public string AirportCode { get; set; }
    }
}
