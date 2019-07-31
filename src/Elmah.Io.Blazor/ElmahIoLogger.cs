using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Elmah.Io.Blazor
{
    public class ElmahIoLogger : ILogger
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly Guid logId;

        public ElmahIoLogger(HttpClient httpClient, string apiKey, Guid logId)
        {
            this.httpClient = httpClient;
            this.apiKey = apiKey;
            this.logId = logId;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var properties = new List<object>();
            if (state is IEnumerable<KeyValuePair<string, object>> stateProperties)
            {
                foreach (var prop in stateProperties)
                {
                    if (prop.Key == "{OriginalFormat}") continue;
                    properties.Add(new { Key = prop.Key, Value = prop.Value?.ToString() });
                }
            }

            if (exception != null && exception.Data != null)
            {
                foreach (var data in exception.Data.Keys)
                {
                    properties.Add(new { Key = data.ToString(), Value = exception.Data[data].ToString() });
                }
            }

            httpClient.SendJsonAsync(HttpMethod.Post, $"https://api.elmah.io/v3/messages/{logId}?api_key={apiKey}", new
            {
                title = formatter(state, exception),
                dateTime = DateTime.UtcNow,
                severity = LogLevelToSeverity(logLevel),
                source = exception?.GetBaseException().Source,
                type = exception?.GetBaseException().GetType().Name,
                detail = exception?.ToString(),
                data = properties,
            });
        }

        private string LogLevelToSeverity(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return "Fatal";
                case LogLevel.Debug:
                    return "Debug";
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Information:
                    return "Information";
                case LogLevel.Trace:
                    return "Verbose";
                case LogLevel.Warning:
                    return "Warning";
                default:
                    return "Information";
            }
        }
    }
}
