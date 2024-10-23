using Microsoft.AspNetCore.SignalR;

namespace EduSchedu.Shared.Infrastructure.Services;

public class ClassChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} joined the chat");
    }

    // Define other methods that clients can call
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}