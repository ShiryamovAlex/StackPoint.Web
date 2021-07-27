using FluentValidation;
using StackPoint.Domain.Models;

namespace StackPoint.Services
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.Phone).NotNull();
            RuleFor(x => x.Email).NotNull().EmailAddress();
        }
    }
}