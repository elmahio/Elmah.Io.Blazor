using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Elmah.Io.Blazor;
using System;

namespace Elmah.Io.Blazor.AspNetCore.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(l => l
                .AddElmahIo("API_KEY", new Guid("LOG_ID")));
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
