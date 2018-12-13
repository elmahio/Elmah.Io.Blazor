# Elmah.Io.Blazor

This is an experimental integration from Blazor to elmah.io. I wanted to see if I could get a WebAssembly-based application to log messages to elmah.io.

To start logging to elmah.io from Blazor, install the [Elmah.Io.Blazor](https://www.nuget.org/packages/Elmah.Io.Blazor/) NuGet package:

```powershell
Install-Package Elmah.Io.Blazor -IncludePrerelease
```

Then configure logging to elmah.io:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddLogging(l => l
        .AddElmahIo("API_KEY", new Guid("LOG_ID")));
    ...
}
```

Messages can be logged by injecting an `ILogger` into each page:

```csharp
@page "/counter"
@using Microsoft.Extensions.Logging
@inject ILogger<Counter> logger

...

@functions {
    int currentCount = 0;

    void IncrementCount()
    {
        currentCount++;
        logger.LogInformation("Incremented count to {currentCount}", currentCount);
    }
}
```

In this example, I log an information message containing the new count.

Exceptions can be logged too:

```csharp
@using Elmah.Io.Blazor.AspNetCore.Shared
@page "/fetchdata"
@using Microsoft.Extensions.Logging
@inject ILogger<Counter> logger
@inject HttpClient Http

...

@functions {
    WeatherForecast[] forecasts;

    protected override async Task OnInitAsync()
    {
        try
        {
            forecasts = await Http.GetJsonAsync<WeatherForecast[]>("api/SampleData/WeatherForecasts-nonexisting");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}
```

In this example, I log the exception happening, when requesting a non-existing json file from the server.
