using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TestConnectDB.Models
{
    public class BookManagementContext : DbContext
    {
        public BookManagementContext(DbContextOptions<BookManagementContext> options) : base(options) { }
        public DbSet<Book> Book { get; set; }

    }
}
