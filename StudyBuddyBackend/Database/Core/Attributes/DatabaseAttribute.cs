using System;

namespace StudyBuddyBackend.Database.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class DatabaseAttribute : Attribute
    {
        protected DatabaseAttribute()
        {
            
        }
    }
}
