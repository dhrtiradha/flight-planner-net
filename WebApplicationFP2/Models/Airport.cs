using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplicationFP2.Models
{
    public class Airport
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Country { get; set; }
        [MaxLength(200)] 
        public string City { get; set; }
        [JsonPropertyName("airport")]
        public string AirportCode { get; set; }
    }
}
