using System.Collections.Generic;
using MediatR;
using StackPoint.Domain.Models;

namespace StackPoint.Service2.Commands
{
    /// <summary>
    /// Запрос пользователей с пагинацией
    /// </summary>
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
        public GetUsersQuery(Paging paging)
        {
            Paging = paging;
        }

        /// <summary>
        /// Пагинация
        /// </summary>
        public Paging Paging { get; }
    }
}