using System.Collections.Generic;
using System.Management.Automation;
using BCAddressGeocoder.Models;
using BCAddressGeocoder.Service;

namespace BCAddressGeocoder.Commands
{
    [Cmdlet(VerbsCommon.Get, "Addresses")]
    [OutputType(typeof(Address))]
    public class GetAddressesCommand : PSCmdlet
    {
        [Parameter(Position = 0)]
        public string AddressString { get; set; }

        [Parameter(Position = 1)]
        [ValidateSet("any", "accessPoint", "frontDoorPoint", "parcelPoint", "rooftopPoint", "routingPoint")]
        public string LocationDescriptor { get; set; } = "any";

        [Parameter(Position = 2)]
        public int MaxResults { get; set; } = 1;

        [Parameter(Position = 3)]
        [ValidateSet("adaptive", "linear", "none")]
        public string Interpolation { get; set; } = "adaptive";

        [Parameter(Position = 4)]
        public bool Echo { get; set; } = true;

        protected override void ProcessRecord()
        {
            var queryParams = new Dictionary<string, object>
            {
                { "addressString", AddressString },
                { "locationDescriptor", LocationDescriptor },
                { "maxResults", MaxResults },
                { "interpolation", Interpolation },
                { "echo", Echo },
            };

            var response = AddressService.GetAddresses(queryParams).GetAwaiter().GetResult();
            foreach (var address in response.Addresses)
            {
                WriteObject(new
                {
                    address.Type,
                    GeometryType = address.Geometry.Type,
                    CRS = $"{address.Geometry.Crs.Type}:{address.Geometry.Crs.Properties.Code}",
                    Coordinates = $"{address.Geometry.Coordinates[1]}, {address.Geometry.Coordinates[0]}",
                    address.Properties.FullAddress,
                    address.Properties.Score,
                    address.Properties.MatchPrecision,
                    address.Properties.PrecisionPoints,
                    address.Properties.SiteName,
                    address.Properties.UnitDesignator,
                    address.Properties.UnitNumber,
                    address.Properties.UnitNumberSuffix,
                    address.Properties.CivicNumber,
                    address.Properties.CivicNumberSuffix,
                    address.Properties.StreetName,
                    address.Properties.StreetType,
                    address.Properties.IsStreetTypePrefix,
                    address.Properties.StreetDirection,
                    address.Properties.IsStreetDirectionPrefix,
                    address.Properties.StreetQualifier,
                    address.Properties.LocalityName,
                    address.Properties.LocalityType,
                    address.Properties.ElectoralArea,
                    address.Properties.ProvinceCode,
                    address.Properties.LocationPositionalAccuracy,
                    address.Properties.LocationDescriptor,
                    address.Properties.SiteID,
                    address.Properties.BlockID,
                    address.Properties.FullSiteDescriptor,
                    address.Properties.AccessNotes,
                    address.Properties.SiteStatus,
                    SiteRetireDate = address.Properties.SiteRetireDate?.ToString("yyyy-MM-dd"),
                    ChangeDate = address.Properties.ChangeDate?.ToString("yyyy-MM-dd"),
                    address.Properties.IsOfficial,
                });
            }
        }
    }
}