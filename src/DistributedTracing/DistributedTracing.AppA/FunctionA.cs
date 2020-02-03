using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace DistributedTracing.AppA
{
    public static class FunctionA
    {
        static readonly Random Random = new Random();
        static HttpClient HttpClient = new HttpClient();

        [FunctionName("FunctionA")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var outboundRequestCount = Random.Next(2, 5);
            for (int i = 1; i <= outboundRequestCount; i++)
            {
                log.LogInformation($"Making outbound request {i} of {outboundRequestCount}.");
                var response = await HttpClient.GetAsync(Environment.GetEnvironmentVariable("FunctionBUrl"));
                log.LogInformation($"Received response status code {(int)response.StatusCode}.");
            }

            return new OkResult();
        }
    }
}
