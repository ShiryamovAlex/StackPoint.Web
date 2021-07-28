using System.Collections.Generic;
using System.Threading.Tasks;
using StackPoint.Domain.Models;

namespace StackPoint.Domain.Services
{
    /// <summary>
    /// Сервис для работы с пользователями в БД
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Добавть нового пользователя
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Id добавленного пользователя</returns>
        Task<long> AddUserAsync(UserDto userDto);

        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <param name="paging">Пагинация</param>
        /// <returns>Список пользователей</returns>
        Task<List<UserDto>> GetAsync(Paging paging);

        /// <summary>
        /// Добавление пользователя в орагнизацию
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="organizationId">Id организации</param>
        Task AddUserToOrganizationAsync(long userId, long organizationId);
    }
}