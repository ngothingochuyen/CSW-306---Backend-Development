using Lab6.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _context;

        public ChatRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessageAsync(ChatMessage message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task SaveLogAsync(HistoryLog log)
        {
            _context.HistoryLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string username)
        {
            return await _context.Messages
                .Where(m => m.MessageType == MessageType.Broadcast ||
                            m.MessageType == MessageType.Group ||
                            (m.MessageType == MessageType.Private && (m.Sender == username || m.Receiver == username)))
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}