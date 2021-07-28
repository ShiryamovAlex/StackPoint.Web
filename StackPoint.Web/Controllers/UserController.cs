using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StackPoint.Domain.Models;
using StackPoint.Web.Commands;

namespace StackPoint.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller 
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int take = 10)
        {
            var getUsersQuery = new GetUsersQuery(page, take);
            var result = await _mediator.Send(getUsersQuery);
            return Ok(result);
        }

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