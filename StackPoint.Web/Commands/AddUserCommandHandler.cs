using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

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
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
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