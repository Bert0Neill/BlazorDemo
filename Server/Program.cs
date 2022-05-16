using BlazorWASMDemo.Server.Middleware;
using BlazorWASMDemo.Server.ORM;
using BlazorWASMDemo.Server.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using BlazorWASMDemo.Server.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

var connectionString = configuration.GetConnectionString("MyMusicShopConnection"); // retrieve DB string
builder.Services.AddDbContext<dbMusicShopContext>(opts => opts.UseSqlServer(connectionString));

builder.Logging.AddConsole();

/*
 * Add SignalR and Response Compression (for performance) Middleware services 
 */
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddScoped<ServerHub>(); // DI SignalR custom class

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();
app.UseResponseCompression();

/*
 * Associate a Global Error handler middleware with all your unhandled exceptions
 */
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<ServerHub>("/serverhub"); // endpoint for the hub
app.MapFallbackToFile("index.html");

app.Run();
