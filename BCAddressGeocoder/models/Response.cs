using System.Collections.Generic;
using Newtonsoft.Json;

namespace BCAddressGeocoder.Models
{
    public class Response
    {
        [JsonProperty("features")]
        public List<Address> Addresses { get; set; }
    }
}