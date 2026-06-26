using Lab6.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab6.Repositories
{
    public interface IChatRepository
    {
        Task SaveMessageAsync(ChatMessage message);
        Task SaveLogAsync(HistoryLog log);
        Task<List<ChatMessage>> GetChatHistoryAsync(string username);
    }
}