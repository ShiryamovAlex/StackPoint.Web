using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StackPoint.Data;
using StackPoint.Data.Models;
using StackPoint.Domain.Models;
using StackPoint.Domain.Services;

namespace StackPoint.Service2.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;

        public UserService(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        public async Task<long> AddUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            await _databaseContext.AddAsync(user);
            await _databaseContext.SaveChangesAsync();

            return user.Id;
        }

        public Task<List<UserDto>> GetAsync(Paging paging) => _databaseContext.Users
            .Skip(paging.Skip)
            .Take(paging.Take)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        public async Task AddUserToOrganizationAsync(long userId, long organizationId)
        {
            var user = await _databaseContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ValidationException($"Не найден пользователь с id {userId}");
            }

            var isOrganizationExisted = await _databaseContext.Organisations
                .Where(x => x.Id == organizationId)
                .AnyAsync();

            if (!isOrganizationExisted)
            {
                throw new ValidationException($"Не найдена организация с id {organizationId}");
            }

            user.OrganisationId = organizationId;

            await _databaseContext.SaveChangesAsync();
        }
    }
}
