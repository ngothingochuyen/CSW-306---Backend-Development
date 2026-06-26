using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Lab6.Hubs;
using Lab6.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ChatDbContext _context;
        private static readonly Dictionary<string, string> OnlineUsers = new Dictionary<string, string>();

        public ChatController(IHubContext<ChatHub> hubContext, ChatDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> JoinRoom([FromBody] RoomRequest request)
        {
            var username = User.Identity?.Name;
            if (!string.IsNullOrEmpty(request.ConnectionId))
            {
                OnlineUsers[request.ConnectionId] = username;
            }

            await _hubContext.Groups.AddToGroupAsync(request.ConnectionId, request.RoomName);
            await _hubContext.Clients.Group(request.RoomName).SendAsync("ReceiveNotification", $"{username} joined the room.");

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LeaveRoom([FromBody] RoomRequest request)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(request.ConnectionId, request.RoomName);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetOnlineUsers()
        {
            return Json(OnlineUsers.Select(u => new { ConnectionId = u.Key, Username = u.Value }));
        }

        [HttpGet]
        public IActionResult GetBroadcastHistory()
        {
            return Json(_context.Messages
                .Where(m => (byte)m.MessageType == 0)
                .OrderBy(m => m.CreatedAt)
                .ToList());
        }

        [HttpGet]
        public IActionResult GetGroupHistory(string roomName)
        {
            return Json(_context.Messages
                .Where(m => (byte)m.MessageType == 1 && m.RoomName == roomName)
                .OrderBy(m => m.CreatedAt)
                .ToList());
        }

        [HttpGet]
        public IActionResult GetPrivateHistory()
        {
            var currentUser = User.Identity?.Name;
            return Json(_context.Messages
                .Where(m => (byte)m.MessageType == 2 && (m.Sender == currentUser || m.Receiver == currentUser))
                .OrderBy(m => m.CreatedAt)
                .ToList());
        }
    }

    public class RoomRequest
    {
        public string ConnectionId { get; set; } = "";
        public string RoomName { get; set; } = "";
    }
}