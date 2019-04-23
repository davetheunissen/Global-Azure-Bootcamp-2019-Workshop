using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace FlightDataFunctionApp
{
    public static class SignalRInfo
    {
        [FunctionName("SignalRInfo")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "flights")] SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            return new OkObjectResult(connectionInfo);
        }
    }
}
