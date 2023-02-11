using Newtonsoft.Json;

namespace BCAddressGeocoder.Models
{
    public class Crs
    {
        public string Type { get; set; }
        [JsonProperty("properties")]
        public CrsProperties Properties { get; set; }
    }
}