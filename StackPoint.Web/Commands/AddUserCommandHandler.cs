using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using StackPoint.Domain.Services;

namespace StackPoint.Web.Commands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, string>
    {
        private const string ConnectionError = "Ошибка добавления нового пользователя. Обратитесь к системному администратору";
        private const string QueueName = "queue:add-user";

        private readonly IBus _bus;
        private readonly IUserValidator _userValidator;

        public AddUserCommandHandler(IBus bus, IUserValidator userValidator)
        {
            _bus = bus;
            _userValidator = userValidator;
        }

        public async Task<string> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var error = _userValidator.CheckUser(request.UserDto);
            if (error != null)
            {
                return error;
            }

            var address = new Uri(QueueName);
            var endpoint = await _bus.GetSendEndpoint(address);
            if (endpoint == null)
            {
                Console.WriteLine($"Не обнаружен слушатель для очереди {QueueName}");
                return ConnectionError;
            }

            await endpoint.Send(request.UserDto, cancellationToken);
            Console.WriteLine($"Создана очередь для создания пользователя - {QueueName}");

            return null;
        }
    }
}