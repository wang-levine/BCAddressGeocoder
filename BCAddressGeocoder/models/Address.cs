namespace BCAddressGeocoder.Models
{
    public class Address
    {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
    }
}