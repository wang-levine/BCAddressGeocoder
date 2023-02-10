using System.Threading.Tasks;
using BCAddressGeocoder.Models;

namespace BCAddressGeocoder.Service
{
    public static class AddressService
    {
        public static async Task<Response> GetAddresses()
        {
            return await UtilService.Get<Response>("/addresses.json");
        }
    }
}