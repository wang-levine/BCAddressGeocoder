using System.Management.Automation;
using BCAddressGeocoder.Models;
using BCAddressGeocoder.Service;

namespace BCAddressGeocoder.Commands
{
    [Cmdlet(VerbsCommon.Get, "Addresses")]
    [OutputType(typeof(Address))]
    public class GetAddressesCommand : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            var response = AddressService.GetAddresses().GetAwaiter().GetResult();
            foreach (var address in response.Addresses)
            {
                WriteObject(address.Properties);
            }
        }
    }
}