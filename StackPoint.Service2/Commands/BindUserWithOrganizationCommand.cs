using MediatR;

namespace StackPoint.Service2.Commands
{
    /// <summary>
    /// Запрос привязки пользователя к организации
    /// </summary>
    public class BindUserWithOrganizationCommand : IRequest<bool>
    {
        public BindUserWithOrganizationCommand(long userId, long organizationId)
        {
            UserId = userId;
            OrganizationId = organizationId;
        }

        public long UserId { get; }

        public long OrganizationId { get; }
    }
}