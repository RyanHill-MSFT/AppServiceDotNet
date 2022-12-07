using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using webjob.Services;

namespace webjob
{
    public class Functions
    {
        private readonly IBackgroundTasker tasker;

        public Functions(IBackgroundTasker tasker)
        {
            this.tasker = tasker;
        }
        
        public void ProcesTimerTrigger([TimerTrigger("*/5 * * * *")] TimerInfo timer, ILogger logger)
        {
            tasker.WorkIt();
            logger.LogInformation("Timer elapsed...");
        }
        
        public void ProcessQueueMessage([QueueTrigger("queue")] string message, ILogger log)
        {
            tasker.WorkIt();
            log.LogInformation(message);
        }
    }
}
