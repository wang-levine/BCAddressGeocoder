using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BCAddressGeocoder.Service
{
    public static class UtilService
    {
        private const string url = "https://geocoder.api.gov.bc.ca";

        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<T> Get<T>(string path)
        {
            using (var httpResponse = await httpClient.GetAsync(url+path, HttpCompletionOption.ResponseHeadersRead))
            {
                httpResponse.EnsureSuccessStatusCode();

                if (!(httpResponse.Content is object) || httpResponse.Content.Headers.ContentType.MediaType != "application/json")
                {
                    return default(T);
                }

                var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                using (var streamReader = new StreamReader(contentStream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        return new JsonSerializer().Deserialize<T>(jsonReader);
                    }
                }
            }
        }
    }
}