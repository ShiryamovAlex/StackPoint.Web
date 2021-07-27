using StackPoint.Domain.Models;

namespace StackPoint.Domain.Services
{
    /// <summary>
    /// Сервис валидации пользователей
    /// </summary>
    public interface IUserValidator
    {
        /// <summary>
        /// Проверить данные пользователя
        /// </summary>
        /// <param name="dto">Dto пользователя</param>
        /// <returns>Возвращает сообщение об ошибке или null, если всё корректно</returns>
        string CheckUser(UserDto dto);
    }
}