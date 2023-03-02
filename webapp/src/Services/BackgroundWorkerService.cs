using webapp.Helper;

namespace webapp.Services
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly ILogger<BackgroundWorkerService> _logger;
        static readonly Guid _guid = Guid.NewGuid();

        public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger)
        {
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWorkerRunning(_guid, Environment.MachineName, DateTimeOffset.Now);
                await Task.Delay(5_000, stoppingToken);
            }
        }
    }
}
