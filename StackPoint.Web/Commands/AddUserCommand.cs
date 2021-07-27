using MediatR;
using StackPoint.Domain.Models;

namespace StackPoint.Web.Commands
{
    /// <summary>
    /// Команда добавления нового пользователя
    /// </summary>
    public class AddUserCommand : IRequest<string>
    {
        public AddUserCommand(UserDto user)
        {
            UserDto = user;
        }

        public UserDto UserDto { get; }
    }
}
