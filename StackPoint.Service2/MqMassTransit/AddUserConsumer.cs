using System;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackPoint.Data;
using StackPoint.Data.Models;
using StackPoint.Domain.Models;

namespace StackPoint.Service2.MqMassTransit
{
    /// <summary>
    /// Потребитель задач на добавление пользователей
    /// </summary>
    public class AddUserConsumer : IConsumer<UserDto>
    {
        private readonly DatabaseContext _databaseContext;
        readonly ILogger<AddUserConsumer> _logger;
        private readonly IMapper _mapper;

        public AddUserConsumer(DatabaseContext databaseContext, ILogger<AddUserConsumer> logger, IMapper mapper)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Consume(ConsumeContext<UserDto> context)
        {
            var userDto = context.Message;
            _logger.LogInformation("Получен пользователь для сохранения: {Name}", userDto.Name);

            try
            {
                var user = _mapper.Map<User>(userDto);

                await _databaseContext.Users.AddAsync(user);
                await _databaseContext.SaveChangesAsync();

                _logger.LogInformation("Новый пользователь добавлен");
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Ошибка добавления пользователя");
            }
        }
    }
}
