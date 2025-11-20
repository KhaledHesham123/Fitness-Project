using IdentityService.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Features.Authantication.Commands.TokenRefresh
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<Result<AuthModel>>> Refresh(string refreshToken)
        {
            var tokenResult = await _mediator.Send(new RefreshTokenCommand(refreshToken));
            if (!tokenResult.Success)
                return Unauthorized(tokenResult);
            return Ok(tokenResult);
        }
    }
}
