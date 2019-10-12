using Newtonsoft.Json;
using StudyBuddyBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyBuddyBackend.Managers
{
    public static class UserManager
    {
        public static IEnumerable<User> GetUsers()
        {
            var queryResult = Database.Instance.ExecuteQuery("SELECT * FROM Users;");
            var users = new List<User>();
            queryResult.ForEach(user => users.Add(EntityManager.Convert<User>(user)));
            return users;
        }
    }
}