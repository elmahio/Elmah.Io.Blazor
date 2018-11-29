using Microsoft.Extensions.Logging;
using System;

namespace Elmah.Io.Blazor
{
    public static class ElmahIoLoggingBuilderExtensions
    {
        public static ILoggingBuilder AddElmahIo(this ILoggingBuilder builder, string apiKey, Guid logId)
        {
            builder.AddProvider(new ElmahIoLoggerProvider(apiKey, logId));
            return builder;
        }
    }
}
