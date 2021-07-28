using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using StackPoint.Domain.Services;

namespace StackPoint.Service2.Commands
{
    public class BindUserWithOrganizationCommandHandler : IRequestHandler<BindUserWithOrganizationCommand, bool>
    {
        private readonly ILogger<BindUserWithOrganizationCommandHandler> _logger;
        private readonly IUserService _userService;

        public BindUserWithOrganizationCommandHandler(ILogger<BindUserWithOrganizationCommandHandler> logger,
            IUserService userService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<bool> Handle(BindUserWithOrganizationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"Получен запрос на добавление пользователя (id = {request.UserId}) в организацию {request.OrganizationId}");

            try
            {
                await _userService.AddUserToOrganizationAsync(request.UserId, request.OrganizationId);
            }
            catch (ValidationException exception)
            {
                _logger.LogError(exception, "Ошибка добавления пользователя");
                return false;
            }

            return true;
        }
    }
}