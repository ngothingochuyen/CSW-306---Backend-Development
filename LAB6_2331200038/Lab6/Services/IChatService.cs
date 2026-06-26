using Lab6.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab6.Services
{
    public interface IChatService
    {
        Task CreateMessageAsync(string sender, string? receiver, string? roomName, string body, MessageType type);
        Task CreateSystemLogAsync(string logType, string message);
        Task<List<ChatMessage>> GetHistoryForUserAsync(string username);
    }
}