using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace EduSchedu.Shared.Infrastructure.Services;

public sealed class ClassChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} joined the chat");
    }
}