using BlazorWASMDemo.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("LogOnServer")]        
        public ActionResult<string> LogOnServer()
        {
            _logger.CaptureExecutionTimeAsTrace("LogOnServer", () =>
            {                
                Thread.Sleep(3000); // perform a server action
            });

            return Ok("Server has logged comment....");
        }
    }
}