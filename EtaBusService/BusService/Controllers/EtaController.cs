using System;
using BusService.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BusService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EtaController : ControllerBase
    {
        // GET /eta/[routeName]/[direction]/[stopName]
        [HttpGet]
        [Route("{routeName}/{direction}/{stopName}")]
        public BusInfo Get(string routeName, string direction, string stopName)
        {
            return GetFirstBus(routeName, direction, stopName);
        }
        private BusInfo GetFirstBus(string routeName, string direction, string stopName)
        {
            Random myRandom = new Random();

            return new BusInfo
            {
                BusID = myRandom.Next(1000, 99999),
                Eta = myRandom.Next(0, 15),
            };
        }
        private IEnumerable<BusInfo> GetAllBuses(string routeName, string direction, string stopName)
        {
            var myRandom = new Random();
            return Enumerable.Range(1, 3).Select(index => new BusInfo
            {
                BusID = myRandom.Next(1000, 99999),
                Eta = myRandom.Next(0, 15),
            })
            .ToArray().OrderBy(oneBus => oneBus.Eta);
        }

    }
}
