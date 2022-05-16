using BlazorWASMDemo.Shared.Extensions;
using BlazorWASMDemo.Server.Middleware;
using BlazorWASMDemo.Server.ORM;
using BlazorWASMDemo.Shared;
using BlazorWASMDemo.Shared.ORM.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly dbMusicShopContext _context;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, dbMusicShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            IEnumerable<WeatherForecast> result = Enumerable.Empty<WeatherForecast>();

            /*
             * using an extension to log (trace) the duration the method took to complete in milliseconds
             */
            _logger.LogInformation("WeatherForecast.Get API Called");

            _logger.CaptureExecutionTimeAsTrace("Get() -> WeatherForecast[]", () =>
            {
                result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            });

            return result;
        }

        [HttpGet("GetAllAlbnums")]        
        public IEnumerable<Album> GetAllAlbnums()
        {
            IEnumerable<Album> result = Enumerable.Empty<Album>();

            _logger.CaptureExecutionTimeAsTrace("GetAllAlbum() -> Album[]", () =>
            {
                result = _context.Set<Album>().Where(alb => alb.Title.Contains("Chronicle")).ToArray();
            });

            return result;
        }
    }
}