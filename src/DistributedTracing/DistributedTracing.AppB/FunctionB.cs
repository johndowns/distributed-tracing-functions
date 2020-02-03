using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DistributedTracing.AppB
{
    public static class FunctionB
    {
        static readonly Random Random = new Random();

        [FunctionName("FunctionB")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var millisecondsDelay = Random.Next(300, 2000);
            log.LogInformation($"Waiting {millisecondsDelay} milliseconds before replying.");
            await Task.Delay(millisecondsDelay);

            return new OkResult();
        }
    }
}
