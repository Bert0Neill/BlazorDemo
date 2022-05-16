using BlazorWASMDemo.Shared.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace BlazorWASMDemo.Server.Hubs
{
    public class ServerHub : Hub
    {
        public ServerHub() { }

        #region save SignalR session connections

        public static ConcurrentDictionary<Guid, UserInfo> userDictionary = new ConcurrentDictionary<Guid, UserInfo>();

        #endregion

        #region Overriden Base Methods

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();            
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            UserInfo userInfo = userDictionary.Single(usr => usr.Value.ConnectionId == Context.ConnectionId).Value;
            userDictionary.Remove(userInfo.UniqueID, out _);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceivedMessage", userInfo?.UserName, 
                $"{userInfo?.UserName} - not monitoring stock market ticker");
        }        

        #endregion
    }
}
