using Blazor.Extensions.Logging;
using Blazored.LocalStorage;
using BlazorWASMDemo.Client;
using BlazorWASMDemo.Client.SignalR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Blazor.Extensions.Logging.BrowserConsoleLogger
builder.Services.AddLogging(builder => builder
    .AddBrowserConsole()
    .SetMinimumLevel(LogLevel.Trace)
);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// add SignalR Hub DI
builder.Services.AddSingleton<ClientHub>();


builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
