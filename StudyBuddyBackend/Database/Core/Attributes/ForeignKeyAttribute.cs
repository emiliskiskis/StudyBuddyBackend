using System;

namespace StudyBuddyBackend.Database.Core.Attributes
{
    public class ForeignKeyAttribute : DatabaseAttribute
    {
        public ForeignKeyAttribute(Type repositoryType)
        {
            RepositoryType = repositoryType;
        }

        public Type RepositoryType { get; }
    }
}
