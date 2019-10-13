using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database
{
    public class UserRepository : CrudRepository<User, string>
    {
        public UserRepository(Database database) : base(database)
        {
        }
    }
}
