using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllInOne.Common.Logging
{
    public class LoggerService<T> : ILoggerService<T> where T : class
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<T> _logger;

        public LoggerService(
            TelemetryClient telemetryClient,
            ILogger<T> logger
        )
        {
            _telemetryClient = telemetryClient;
            _logger = logger;
        }


        public void LogTrace(string message)
        {
            _logger.LogTrace(message);
        }

        public void LogTrace(string message, IDictionary<string, string> properties = null)
        {
            TrackMessage(message, LogLevel.Trace, properties);
            LogTrace(message);
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        public void LogDebug(string message, IDictionary<string, string> properties = null)
        {
            TrackMessage(message, LogLevel.Debug, properties);
            LogDebug(message);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogInformation(string message, IDictionary<string, string> properties = null)
        {
            TrackMessage(message, LogLevel.Information, properties);
            LogInformation(message);
        }

        public void LogMetric(string key, double value, LogLevel logLevel = LogLevel.Trace)
        {
            _telemetryClient.GetMetric(key).TrackValue(value);
            Flush();
            _logger.Log(logLevel, $"{key}: {value}");
        }

        public void LogMetric(string key, TimeSpan value, LogLevel logLevel = LogLevel.Trace)
        {
            _telemetryClient.GetMetric(key + "Ms").TrackValue(value.TotalMilliseconds);
            _telemetryClient.GetMetric(key + "Sec").TrackValue(value.TotalSeconds);
            Flush();
            _logger.Log(logLevel, $"{key}: {value.ToString(@"hh\:mm\:ss\:fff")}");
        }

        public void LogEvent(
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null,
            LogLevel logLevel = LogLevel.Trace
        )
        {
            _telemetryClient.TrackEvent(eventName, properties, metrics);
            Flush();
            _logger.Log(
                logLevel,
                $"{eventName}, " +
                $"{properties?.Select(p => $"{p.Key}:'{p.Value}'").Aggregate((s1, s2) => $"{s1}, {s2}")}" +
                $"{metrics?.Select(m => $"{m.Key}:'{m.Value}'").Aggregate((s1, s2) => $"{s1}, {s2}")}"
            );
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogWarning(string message, IDictionary<string, string> properties = null)
        {
            TrackMessage(message, LogLevel.Warning, properties);
            LogWarning(message);
        }

        public void LogError(string message)
        {
            TrackMessage(message, LogLevel.Error);
            _logger.LogError(message);
        }

        public void LogError(string message, Exception exception)
        {
            TrackMessage(message, LogLevel.Error);
            TrackException(exception);
            _logger.LogError(exception, message);
        }

        public void LogError(string message, Exception exception, IDictionary<string, string> properties = null)
        {
            TrackMessage(message, LogLevel.Error);
            TrackException(exception, properties);
            _logger.LogError(exception, message);
        }

        public void LogCritical(string message)
        {
            TrackMessage(message, LogLevel.Critical);
            _logger.LogCritical(message);
        }

        public void LogCritical(string message, Exception exception)
        {
            TrackMessage(message, LogLevel.Critical);
            TrackException(exception);
            _logger.LogCritical(exception, message);
        }

        public void LogCritical(string message, Exception exception, IDictionary<string, string> properties = null)
        {
            TrackException(exception, properties);
            _logger.LogCritical(exception, message);
        }

        public void Flush()
        {
            _telemetryClient.Flush();
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            return new ValueTask(Task.Run(() => Flush()));
        }

        #region Private
        private void TrackMessage(string message, LogLevel logLevel, IDictionary<string, string> properties = null)
        {
            _telemetryClient.TrackTrace($"{logLevel}:{message}", properties);
            Flush();
        }
        private void TrackException(Exception exception, IDictionary<string, string> properties = null)
        {
            _telemetryClient.TrackException(exception, properties);
            Flush();
        }
        #endregion
    }
}
