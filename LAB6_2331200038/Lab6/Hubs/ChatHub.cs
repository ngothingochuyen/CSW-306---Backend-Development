using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Lab6.Models;
using Lab6.Services;
using System.Threading.Tasks;

namespace Lab6.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string message)
        {
            var sender = Context.User?.Identity?.Name ?? "Anonymous";
            await _chatService.CreateMessageAsync(sender, null, null, message, MessageType.Broadcast);
            await Clients.All.SendAsync("ReceiveMessage", sender, message);
        }

        public async Task SendGroupMessage(string roomName, string message)
        {
            var sender = Context.User?.Identity?.Name ?? "Anonymous";
            await _chatService.CreateMessageAsync(sender, null, roomName, message, MessageType.Group);
            await Clients.Group(roomName).SendAsync("ReceiveGroupMessage", roomName, sender, message);
        }

        public async Task SendPrivateMessage(string receiverConnectionId, string receiverName, string message)
        {
            var sender = Context.User?.Identity?.Name ?? "Anonymous";
            await _chatService.CreateMessageAsync(sender, receiverName, null, message, MessageType.Private);
            await Clients.Client(receiverConnectionId).SendAsync("ReceivePrivateMessage", Context.ConnectionId, sender, message);
        }
    }
}