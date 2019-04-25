using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FlightDataFunctionApp
{
    public static class FlightDataChangeFeed
    {
        [FunctionName("FlightDataChangeFeed")]
        public static async Task RunAsync([CosmosDBTrigger(
            databaseName: "flightsdb",
            collectionName: "flights",
            ConnectionStringSetting = "AzureCosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [SignalR(HubName = "flightdata")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                foreach (var flight in input)
                {
                    await signalRMessages.AddAsync(new SignalRMessage
                    {
                        Target = "newFlightData",
                        Arguments = new[] { flight }
                    });
                }
            }
        }
    }
}
