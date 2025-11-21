using IdentityService.Authorization;
using IdentityService.Features.Shared;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.Features.Authantication.Commands.Logout
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public AuthController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }


        [HttpPost("Logout")]
        [Authorize]
        public async Task<ActionResult<Result<bool>>> Logout()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _mediator.Send(new LogoutCommand(
                 userId
             ));
            await _unitOfWork.SaveChangesAsync();

            return StatusCode(response.StatusCode, response);
        }
    }
}
