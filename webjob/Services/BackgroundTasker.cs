using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace webjob.Services
{
    public interface IBackgroundTasker
    {
        void WorkIt();
    }

    public class BackgroundTasker : IBackgroundTasker
    {
        private readonly ILogger<BackgroundTasker> logger;

        public BackgroundTasker(ILogger<BackgroundTasker> logger)
        {
            this.logger = logger;
        }

        public void WorkIt()
        {
            logger.LogInformation("It's working...");
        }
    }
}
