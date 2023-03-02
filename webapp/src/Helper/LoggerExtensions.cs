namespace webapp.Helper {
    public static partial class LoggerExtensions {
        [LoggerMessage(Level = LogLevel.Information, Message = "{Instance} worker running on {Machine} at {time}")]
        public static partial void LogWorkerRunning(this ILogger logger, Guid instance, string machine, DateTimeOffset time);
    }
}
