using Microsoft.AspNetCore.SignalR;

namespace ChartAPI.Hubs
{
    public class SalesHub : Hub
    {
        public async Task SendMessageAsync()
        {
            await Clients.All.SendAsync("receiveMessage", "Hello");
        }
    }
}
