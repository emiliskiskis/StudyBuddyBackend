using Microsoft.EntityFrameworkCore;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
        }
     
        public DbSet<User> Users { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UserInChat> UsersInChats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<UserSubject> UserSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasKey("Username", "ChatId", "SentAt");
            modelBuilder.Entity<Message>().Property(m => m.SentAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<UserInChat>().HasKey("ChatId", "Username");
            modelBuilder.Entity<UserSubject>().HasKey("SubjectName", "Username");
            modelBuilder.Entity<Feedback>().HasKey("AuthorUsername", "ReviewerUsername");
        }
    }
}
