using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FlightDataFunctionApp
{
    public static class FlightDataPoll
    {
        static HttpClient client = new HttpClient();

        [FunctionName("FlightDataPoll")]
        public static async Task RunAsync(
            [TimerTrigger("*/5 * * * * *")]TimerInfo myTimer,
            [CosmosDB(
            databaseName: "flightsdb",
            collectionName: "flights",
            ConnectionStringSetting = "AzureCosmosDBConnection")]IAsyncCollector<Flight> documents,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var openSkyUrl = "https://opensky-network.org/api/states/all?lamin=-50.00&lomin=160.00&lamax=-30.00&lomax=180.00";

            using (HttpResponseMessage res = await client.GetAsync(openSkyUrl))
            using (HttpContent content = res.Content)
            {
                var result = JsonConvert.DeserializeObject<Rootobject>(await content.ReadAsStringAsync());
                foreach (var item in result.states)
                {
                    await documents.AddAsync(Flight.CreateFromData(item));
                }

                log.LogInformation($"Total flights processed {result.states.Length}");
            }
        }
    }
}
