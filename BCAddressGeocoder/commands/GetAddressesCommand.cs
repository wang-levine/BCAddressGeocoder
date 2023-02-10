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
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string AddressString { get; set; }

        protected override void ProcessRecord()
        {
            var queryParams = new Dictionary<string, string>
            {
                { "addressString", AddressString },
            };

            var response = AddressService.GetAddresses(queryParams).GetAwaiter().GetResult();
            foreach (var address in response.Addresses)
            {
                WriteObject(new
                {
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
                });
            }
        }
    }
}