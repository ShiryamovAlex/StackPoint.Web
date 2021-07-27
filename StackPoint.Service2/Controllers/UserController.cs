using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackPoint.Data;
using StackPoint.Domain.Models;
using StackPoint.Service2.AutoMaps;

namespace StackPoint.Service2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DatabaseContext _databaseContext;

        public UserController(ILogger<UserController> logger, DatabaseContext databaseContext)
        {
            _logger = logger;
            _databaseContext = databaseContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Paging paging)
        {
            var provider = new MapperConfiguration(expression => expression.AddProfile(new UserProfile()));
            var users = await _databaseContext.Users
                .Skip(paging.Skip)
                .Take(paging.Take)
                .ProjectTo<UserDto>(provider)
                .ToListAsync();

            return Ok(users);
        }
    }
}
