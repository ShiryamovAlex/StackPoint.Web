using System;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackPoint.Data;
using StackPoint.Data.Models;
using StackPoint.Domain.Models;
using StackPoint.Service2.AutoMaps;

namespace StackPoint.Service2.MqMassTransit
{
    /// <summary>
    /// Потребитель задач на добавление пользователей
    /// </summary>
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

            var transaction = _databaseContext.Database.BeginTransaction();
            try
            {
                var config = new MapperConfiguration(expression => expression.AddProfile(new UserProfile()));
                var mapper = new Mapper(config);
                var user = mapper.Map<User>(userDto);

                await _databaseContext.Users.AddAsync(user);

                await _databaseContext.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Новый пользователь добавлен");
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, "Ошибка добавления пользователя");
                await transaction.RollbackAsync();
            }
        }
    }
}
