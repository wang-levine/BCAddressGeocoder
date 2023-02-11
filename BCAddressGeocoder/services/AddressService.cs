using System.Collections.Generic;
using System.Threading.Tasks;
using BCAddressGeocoder.Models;

namespace BCAddressGeocoder.Service
{
    public static class AddressService
    {
        public static async Task<Response> GetAddresses(Dictionary<string, object> queryParams)
        {
            return await UtilService.Get<Response>("/addresses.json", queryParams);
        }
    }
}