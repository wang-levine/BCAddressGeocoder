using System.Collections.Generic;

namespace BCAddressGeocoder.Models
{
    public class Geometry
    {
        public string Type { get; set; }
        public Crs Crs { get; set; }
        public List<decimal> Coordinates { get; set; }
    }
}