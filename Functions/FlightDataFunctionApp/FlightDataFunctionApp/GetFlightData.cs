using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FlightDataFunctionApp
{
    public static class GetFlightData
    {
        [FunctionName("GetFlightData")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "flightsdb",
                collectionName: "flights",
                ConnectionStringSetting = "AzureCosmosDBConnection",
                SqlQuery = "SELECT * FROM c")] IEnumerable<Flight> Flights,
            ILogger log)
        {
            log.LogInformation("GetFlightData processed a request");
            return new OkObjectResult(Flights);
        }
    }
}
