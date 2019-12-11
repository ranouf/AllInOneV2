using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AllInOne.Common.Logging
{
    public interface ILoggerService : IAsyncDisposable
    {
        void LogTrace(string message);
        void LogTrace(string message, IDictionary<string, string> properties = null);
        void LogDebug(string message);
        void LogDebug(string message, IDictionary<string, string> properties = null);
        void LogInformation(string message);
        void LogInformation(string message, IDictionary<string, string> properties = null);
        void LogWarning(string message);
        void LogWarning(string message, IDictionary<string, string> properties = null);
        void LogError(string message);
        void LogError(string message, Exception exception);
        void LogError(string message, Exception exception, IDictionary<string, string> properties = null);
        void LogCritical(string message);
        void LogCritical(string message, Exception exception);
        void LogCritical(string message, Exception exception, IDictionary<string, string> properties = null);
        void LogMetric(string key, double value, LogLevel logLevel = LogLevel.Trace);
        void LogMetric(string key, TimeSpan value, LogLevel logLevel = LogLevel.Trace);
        void LogEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null, LogLevel logLevel = LogLevel.Trace);
        void Flush();
    }

    public interface ILoggerService<T> : ILoggerService
    {
    }
}
