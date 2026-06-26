using Lab6.Models;
using Lab6.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab6.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;

        public ChatService(IChatRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateMessageAsync(string sender, string? receiver, string? roomName, string body, MessageType type)
        {
            var message = new ChatMessage
            {
                Sender = sender,
                Receiver = receiver,
                RoomName = roomName,
                MessageBody = body,
                MessageType = type,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.SaveMessageAsync(message);
        }

        public async Task CreateSystemLogAsync(string logType, string message)
        {
            var log = new HistoryLog
            {
                LogType = logType,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.SaveLogAsync(log);
        }

        public async Task<List<ChatMessage>> GetHistoryForUserAsync(string username)
        {
            return await _repository.GetChatHistoryAsync(username);
        }
    }
}