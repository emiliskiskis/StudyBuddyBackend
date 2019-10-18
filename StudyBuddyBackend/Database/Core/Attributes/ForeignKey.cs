using System;

namespace StudyBuddyBackend.Database.Core.Attributes
{
    public class ForeignKey : DatabaseAttribute
    {
        public ForeignKey(Type repositoryType)
        {
            RepositoryType = repositoryType;
        }

        public Type RepositoryType { get; }
    }
}
