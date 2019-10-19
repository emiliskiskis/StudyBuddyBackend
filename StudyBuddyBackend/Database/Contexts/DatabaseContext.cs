using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().Property(m => m.SentAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
