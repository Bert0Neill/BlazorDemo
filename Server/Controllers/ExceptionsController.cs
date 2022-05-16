using BlazorWASMDemo.Shared.Extensions;
using BlazorWASMDemo.Server.ORM;
using BlazorWASMDemo.Shared;
using BlazorWASMDemo.Shared.ORM.Models;
using Microsoft.AspNetCore.Mvc;
using BlazorWASMDemo.Server.Middleware;

namespace BlazorWASMDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExceptionsController : ControllerBase
    {
        private readonly dbMusicShopContext _context;
        private readonly ILogger<ExceptionsController> _logger;

        public ExceptionsController(ILogger<ExceptionsController> logger, dbMusicShopContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        [HttpGet("GlobalExceptionHandle_General")]        
        public void GlobalExceptionHandleGeneral()
        {
            _logger.CaptureExecutionTimeAsTrace("GlobalExceptionHandle_General", () =>
            {
                this.CaptureGeneralException();
            });
        }

        [HttpGet("GlobalExceptionHandle_Yourself")]
        public void GlobalExceptionGeneral()
        {
            _logger.CaptureExecutionTimeAsTrace("GlobalExceptionHandle_Yourself", () =>
            {
                this.CaptureGeneralExExceptionButHandle();
            });
        }

        [HttpGet("GlobalExceptionHandle_Custom")]
        public void GlobalExceptionCustom()
        {
            _logger.CaptureExecutionTimeAsTrace("GlobalExceptionHandle_Custom", () =>
            {
                this.CaptureCustomException();
            });
        }

        private void CaptureGeneralException()
        {
            _logger.LogInformation("Dividing by zero....");

            // expect a divide by zero exception
            double value = (1 * 100 / int.Parse("0"));
        }

        private void CaptureCustomException()
        {
            _logger.LogInformation("Custom exception class....");

            // a custom exception that will return a 400 response
            throw new Version1CustomException("No coffee left in the canteen!!!");
        }

        private void CaptureGeneralExExceptionButHandle()
        {
            _logger.LogInformation("You want to handle this exception specifically yourself....");

            // a key not found exception that will return a 404 response
            throw new KeyNotFoundException("Account details not found");
        }
    }
}