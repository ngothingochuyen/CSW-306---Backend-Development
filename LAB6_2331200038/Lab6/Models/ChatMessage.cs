using Microsoft.EntityFrameworkCore;
using System;

namespace Lab6.Models
{
    public enum MessageType : byte
    {
        Broadcast = 0,
        Group = 1,
        Private = 2
    }

    public class ChatMessage
    {
        public int Id { get; set; }
        public string Sender { get; set; } = string.Empty;
        public string? Receiver { get; set; }
        public string? RoomName { get; set; }
        public string MessageBody { get; set; } = string.Empty;
        public MessageType MessageType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class HistoryLog
    {
        public int Id { get; set; }
        public string LogType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<HistoryLog> HistoryLogs { get; set; }
    }
}