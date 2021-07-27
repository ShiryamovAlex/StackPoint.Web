using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;
using StackPoint.Data;
using StackPoint.Data.Models;
using StackPoint.Domain.Models;

namespace StackPoint.Service2.MqMassTransit
{
    public class AddUserConsumerDefinition : ConsumerDefinition<AddUserConsumer>
    {
        public AddUserConsumerDefinition()
        {
            EndpointName = "input-queue";
            ConcurrentMessageLimit = 8;
        }
    }

    public class AddUserConsumer : IConsumer<UserDto>
    {
        private readonly DatabaseContext _databaseContext;
        readonly ILogger<AddUserConsumer> _logger;

        public AddUserConsumer(DatabaseContext databaseContext, ILogger<AddUserConsumer> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserDto> context)
        {
            var userDto = context.Message;
            _logger.LogInformation("Получен пользователь для сохранения: {Name}", userDto.Name);

            await _databaseContext.Users.AddAsync(new User
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                Patronymic = userDto.Patronymic,
                Email = userDto.Email,
                Phone = userDto.Phone
            });

            await _databaseContext.SaveChangesAsync();

            _logger.LogInformation("Новый пользователь добавлен");
        }
    }
}
