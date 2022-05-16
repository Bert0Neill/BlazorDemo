using BlazorWASMDemo.Server.Hubs;
using BlazorWASMDemo.Server.ORM;
using BlazorWASMDemo.Shared.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.Json;

namespace BlazorWASMDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignalRController : ControllerBase
    {
        private readonly dbMusicShopContext _context;
        private readonly ILogger<SignalRController> _logger;
        private readonly ServerHub _serverHub;
        private readonly IHubContext<ServerHub> _hubContext;

        public SignalRController(ILogger<SignalRController> logger, dbMusicShopContext context, ServerHub serverHub, IHubContext<ServerHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _serverHub = serverHub;
            _hubContext = hubContext;
        }

        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [HttpPost("RegisterUser")]
        public ActionResult<UserInfo> RegisterUser([FromBody] string jsonUserInfo)
        {
            try
            {
                if (String.IsNullOrEmpty(jsonUserInfo)) return BadRequest(Resources.Resource.NullUserMessage);

                UserInfo userInfo = JsonSerializer.Deserialize<UserInfo>(jsonUserInfo, jsonSerializerOptions);

                // if the connection already exists - update the connection ID
                //if (_serverHub.userDictionary.ContainsKey(userInfo.UniqueID)) userInfo.IsRegistered = true;
                if (ServerHub.userDictionary.ContainsKey(userInfo.UniqueID)) userInfo.IsRegistered = true;

                //_serverHub.userDictionary.AddOrUpdate(userInfo.UniqueID, userInfo, (key, oldValue) => userInfo); // key
                ServerHub.userDictionary.AddOrUpdate(userInfo.UniqueID, userInfo, (key, oldValue) => userInfo); // key

                // send only to the caller
                _hubContext.Clients.Client(userInfo.ConnectionId).SendAsync("LandingPageReceiveMessage", 
                    $"Registered with unique ID {userInfo.UniqueID}."); // push back to caller

                return Ok(userInfo);

            }
            catch (Exception ex)
            {
                return BadRequest(String.Format(Resources.Resource.InvalidProcessUserMessage, ex.Message));
            }
        }

        [HttpPost("UpdateRegisteredUser")]
        public ActionResult<UserInfo> UpdateRegisteredUser([FromBody] string jsonUserInfo)
        {
            if (String.IsNullOrEmpty(jsonUserInfo)) return BadRequest(Resources.Resource.NullUserMessage);

            UserInfo userInfo = JsonSerializer.Deserialize<UserInfo>(jsonUserInfo, jsonSerializerOptions);
            userInfo.IsRegistered = true; // update registration status

            //_serverHub.userDictionary.AddOrUpdate(userInfo.UniqueID, userInfo, (key, oldValue) => userInfo); // key
            ServerHub.userDictionary.AddOrUpdate(userInfo.UniqueID, userInfo, (key, oldValue) => userInfo); // key

            // notify all that somebody has joined (execpt this caller)
            _hubContext.Clients.AllExcept(userInfo.ConnectionId).SendAsync("ReceiveReportsMessage", $"User '{userInfo.UserName}' - is now monitoring stock market data");

            Random rnd = new();
            SignalrData signalrData = new();

            // generate some initial random stock data
            signalrData.Data.Add(rnd.Next(1, 10));
            signalrData.Data.Add(rnd.Next(signalrData.Data[0], signalrData.Data[0] + 10));
            signalrData.Data.Add(rnd.Next(signalrData.Data[1], signalrData.Data[1] + 10));
            signalrData.Data.Add(rnd.Next(signalrData.Data[2], signalrData.Data[2] + 10));
            signalrData.Data.Add(rnd.Next(signalrData.Data[3], signalrData.Data[3] + 10));
            signalrData.Data.Add(100 - rnd.Next(signalrData.Data[0] + signalrData.Data[1] + signalrData.Data[2] + signalrData.Data[3] + signalrData.Data[4]));

            // send this mock data to caller only
            _hubContext.Clients.Client(userInfo.ConnectionId).SendAsync("ReceivedStockUpdates", signalrData); // push data to caller only

            return Ok(userInfo);
        }

        [HttpGet("UpdateReports")]
        public ActionResult UpdateReports()
        {
            Random rnd = new();
            SignalrData signalrData = new();

            // generate random stock data
            signalrData.Data.Add(rnd.Next(1, 10));
            signalrData.Data.Add(rnd.Next(signalrData.Data[0], signalrData.Data[0] + 10));
            signalrData.Data.Add(rnd.Next(signalrData.Data[1], signalrData.Data[1] + 10));
            signalrData.Data.Add(rnd.Next(signalrData.Data[2], signalrData.Data[2] + 10));
            signalrData.Data.Add(rnd.Next(signalrData.Data[3], signalrData.Data[3] + 10));
            signalrData.Data.Add(100 - rnd.Next(signalrData.Data[0] + signalrData.Data[1] + signalrData.Data[2] + 
                signalrData.Data[3] + signalrData.Data[4]));

            // send to all connected sessions
            _hubContext.Clients.All.SendAsync("ReceivedStockUpdates", signalrData);

            return Ok();
        }
    }
}