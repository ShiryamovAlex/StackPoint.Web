using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using StackPoint.Services;

namespace StackPoint.Web.Commands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, string>
    {
        private const string ConnectionError = "Ошибка добавления нового пользователя. Обратитесь к системному администратору";
        private const string QueueName = "queue:add-user";

        private readonly IBus _bus;
        private readonly ILogger _logger;

        public AddUserCommandHandler(IBus bus, ILogger<AddUserCommandHandler> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var userValidator = new UserValidator();
            var validationResult = userValidator.Validate(request.UserDto);
            if (!validationResult.IsValid)
            {
                var error = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                _logger.Log(LogLevel.Error, error);
                return error;
            }

            var address = new Uri(QueueName);
            var endpoint = await _bus.GetSendEndpoint(address);
            if (endpoint == null)
            {
                _logger.Log(LogLevel.Error, $"Не обнаружен слушатель для очереди {QueueName}");
                return ConnectionError;
            }

            await endpoint.Send(request.UserDto, cancellationToken);

            _logger.Log(LogLevel.Information, $"Создана очередь для создания пользователя - {QueueName}");

            return null;
        }
    }
}