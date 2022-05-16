using BlazorWASMDemo.Shared.Extensions;
using BlazorWASMDemo.Server.ORM;
using BlazorWASMDemo.Shared;
using BlazorWASMDemo.Shared.ORM.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWASMDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyMusicStoreController : ControllerBase
    {
        private readonly dbMusicShopContext _context;
        private readonly ILogger<MyMusicStoreController> _logger;

        public MyMusicStoreController(ILogger<MyMusicStoreController> logger, dbMusicShopContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        [HttpGet("GetFilteredAlbnums")]        
        public IEnumerable<Album> GetFilteredAlbnums(string filterByArtistName)
        {
            IEnumerable<Album> result = Enumerable.Empty<Album>();

            _logger.CaptureExecutionTimeAsTrace("GetFilteredAlbnums(filterByArtistName) -> Album[]", () =>
            {
                int[] artistID = _context.Set<Artist>().Where(alb => alb.Name.Contains(filterByArtistName)).Select(srt=>srt.ArtistId).ToArray();

                if (artistID is not null)
                {
                    result = _context.Set<Album>().Where(alb => artistID.Contains(alb.ArtistId)).ToArray();
                }
            });

            return result;
        }
    }
}