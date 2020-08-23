using System;
using System.Net.Http;
using System.Threading.Tasks;
using BusClient.Models;
using Newtonsoft.Json;

namespace BusClient
{
    public class BusServiceClient
    {
        private readonly HttpClient httpClient;

        public BusServiceClient(string busServiceUri)
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(busServiceUri)
            };
        }

        public async Task<BusInfo> GetBusInfo(string routeName, string direction, string stopName)
        {
            var requestUri = $"/eta/{routeName}/{direction}/{stopName}";
            var response = await httpClient.GetAsync(requestUri);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Content: " + content);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<BusInfo>(content);
        }
    }


}