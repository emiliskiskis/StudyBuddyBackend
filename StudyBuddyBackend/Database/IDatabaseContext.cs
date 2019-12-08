using Microsoft.EntityFrameworkCore;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database
{
    public interface IDatabaseContext
    {
        DbSet<User> Users { get; set; }
        DbSet<ProfilePicture> ProfilePictures { get; set; }
        DbSet<Chat> Chats { get; set; }
        DbSet<UserInChat> UsersInChats { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Feedback> Feedback { get; set; }
        DbSet<Subject> Subjects { get; set; }
        DbSet<TeacherSubject> TeacherSubjects { get; set; }
        DbSet<TeacherInfo> TeacherInfo { get; set; }
        int SaveChanges();
    }
}
