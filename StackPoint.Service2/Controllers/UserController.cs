using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StackPoint.Domain.Models;
using StackPoint.Service2.Commands;

namespace StackPoint.Service2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Paging paging)
        {
            var getUsersQuery = new GetUsersQuery(paging);
            var result = await _mediator.Send(getUsersQuery);

            return Ok(result);
        }

        public async Task<IActionResult> BindUserWithOrganizationAsync(long userId, long organizationId)
        {
            var command = new BindUserWithOrganizationCommand(userId, organizationId);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
