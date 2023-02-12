using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace BCAddressGeocoder.Service
{
    public static class UtilService
    {
        private const string url = "https://geocoder.api.gov.bc.ca";

        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<T> Get<T>(string path, Dictionary<string, object> queryParams)
        {
            // building uri
            StringBuilder uri = new StringBuilder();
            uri.Append(url);
            uri.Append(path);
            int i = 0;
            foreach (var queryParam in queryParams)
            {
                if (queryParam.Value is string && string.IsNullOrEmpty(queryParam.Value.ToString()))
                {
                    continue;
                }
                uri.Append(++i == 1 ? "?" : "&");
                uri.Append(queryParam.Key);
                uri.Append("=");
                uri.Append(HttpUtility.UrlEncode(queryParam.Value.ToString()));
            }

            using (var httpResponse = await httpClient.GetAsync(uri.ToString(), HttpCompletionOption.ResponseHeadersRead))
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