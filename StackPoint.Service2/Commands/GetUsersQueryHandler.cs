using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackPoint.Data;
using StackPoint.Domain.Models;

namespace StackPoint.Service2.Commands
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, DatabaseContext databaseContext, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var paging = request.Paging;
            var users = await _databaseContext.Users
                .Skip(paging.Skip)
                .Take(paging.Take)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Выполнено запрос получения пользователей");

            return users;
        }
    }
}
