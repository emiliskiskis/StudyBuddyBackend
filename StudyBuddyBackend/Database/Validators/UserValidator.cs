using FluentValidation;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Salt).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
