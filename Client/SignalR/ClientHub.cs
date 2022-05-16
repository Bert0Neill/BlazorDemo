using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorWASMDemo.Client.SignalR
{
    public class ClientHub
    {
        public HubConnection? hubConnection { get; set; } = null;
        public Guid UniqueID { get; private set; } = Guid.NewGuid();

        public ClientHub() { }

    }
}
