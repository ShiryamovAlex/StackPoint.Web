using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using StackPoint.Domain.Models;
using StackPoint.Domain.Services;

namespace StackPoint.Service2.MqMassTransit
{
    /// <summary>
    /// Потребитель задач на добавление пользователей
    /// </summary>
    public class AddUserConsumer : IConsumer<UserDto>
    {
        private readonly IUserService _userService;
        readonly ILogger<AddUserConsumer> _logger;

        public AddUserConsumer(ILogger<AddUserConsumer> logger, IUserService userService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task Consume(ConsumeContext<UserDto> context)
        {
            var userDto = context.Message;
            _logger?.LogInformation("Получен пользователь для сохранения: {Name}", userDto.Name);

            try
            {
                await _userService.AddUserAsync(userDto);

                _logger?.LogInformation("Новый пользователь добавлен");
            }
            catch(Exception exception)
            {
                _logger?.LogError(exception, "Ошибка добавления пользователя");
            }
        }
    }
}
