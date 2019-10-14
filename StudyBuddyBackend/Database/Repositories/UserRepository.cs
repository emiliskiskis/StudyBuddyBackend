using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Repositories
{
    public class UserRepository : CrudRepository<User, string>
    {
        public UserRepository(Database database, ILogger<UserRepository> logger) : base(database, logger)
        {
        }
    }
}
