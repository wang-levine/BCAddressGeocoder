using Newtonsoft.Json;

namespace BCAddressGeocoder.Models
{
    public class Fault
    {
        public string Value { get; set; }
        public string Element { get; set; }
        [JsonProperty("fault")]
        public string FaultValue { get; set; }
        public int Penalty { get; set; }
    }
}