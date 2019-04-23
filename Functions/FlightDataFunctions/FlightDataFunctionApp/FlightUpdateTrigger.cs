using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FlightDataFunctionApp
{
    public static class FlightUpdateTrigger
    {
        [FunctionName("FlightUpdateTrigger")]
        public static async Task RunAsync(
            [CosmosDBTrigger(
                databaseName: "flightsdb",
                collectionName: "flights",
                ConnectionStringSetting = "CosmosDBConnection",
                LeaseCollectionName = "leases",
                CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> updatedFlights,
            [SignalR(HubName = "flights")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            if (updatedFlights != null && updatedFlights.Count > 0)
            {
                log.LogInformation("Documents modified " + updatedFlights.Count);
                foreach (var flight in updatedFlights)
                {
                    await signalRMessages.AddAsync(new SignalRMessage
                    {
                        Target = "flightsUpdated",
                        Arguments = new[] { flight }
                    });
                }
            }
        }
    }
}
