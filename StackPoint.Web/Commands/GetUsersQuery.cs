using System.Collections.Generic;
using MediatR;
using StackPoint.Domain.Models;

namespace StackPoint.Web.Commands
{
    /// <summary>
    /// Запрос пользователей с пагинацией
    /// </summary>
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
        public GetUsersQuery(int page, int take)
        {
            Page = page;
            Take = take;
        }

        /// <summary>
        /// Страница
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// Необходимое количество
        /// </summary>
        public int Take { get; }
    }
}
