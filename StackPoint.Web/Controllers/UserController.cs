using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackPoint.Domain.Models;
using StackPoint.Web.Commands;

namespace StackPoint.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller 
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;

        public UserController(ILogger<UserController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int take = 10)
        {
            var getUsersQuery = new GetUsersQuery(page, take);
            var result = await _mediator.Send(getUsersQuery);
            return Ok(result);
        }

        /// <summary>
        /// Создание нового объекта
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserDto dto = null)
        {
            var addUserCommand = new AddUserCommand(dto);
            var result = await _mediator.Send(addUserCommand);
            return result == null ? (IActionResult)NoContent() : Fail(result);
        }

        private static ObjectResult Fail(string message) => new ObjectResult(new
        {
            success = false,
            message
        })
        {
            StatusCode = 422
        };
    }
}