using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace Elmah.Io.Blazor
{
    public class ElmahIoLoggerProvider : ILoggerProvider
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly Guid logId;

        public ElmahIoLoggerProvider(string apiKey, Guid logId)
        {
            httpClient = new HttpClient();
            this.apiKey = apiKey;
            this.logId = logId;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new ElmahIoLogger(httpClient, apiKey, logId);
        }

        public void Dispose()
        {
        }
    }
}
