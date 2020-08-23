using System;
using System.Net.Http;
using BusClient.Models;

namespace BusClient
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                int port = 5000;
                var baseUri = $"http://localhost:{port}";

                string stopName = "opera";
                string routeName = "20";
                string direction = "nord";

                Console.WriteLine("baseUri " + baseUri);
                var result = new BusServiceClient(baseUri).GetBusInfo(routeName, direction, stopName).GetAwaiter().GetResult();
                Console.WriteLine(result.Eta);
                
                Console.WriteLine(result.BusGpsLocation.Latitude+result.BusGpsLocation.Longitude);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get eta, e=" + ex.Message);
            }
        }
    }
}
