using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace hello {
    public class Functions {
        [FunctionName("ProcessMessage")]
        public void RunProcessMessage([QueueTrigger("%QueueSettings__QueueName", Connection = "AzureWebJobStorage")] string myQueueItem, ILogger log) {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }

        // timer trigger function
        [FunctionName("TimerTrigger")]
        public void RunTimerTrigger([TimerTrigger("* 9-20 * * 1-5")] TimerInfo myTimer, ILogger log) {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
