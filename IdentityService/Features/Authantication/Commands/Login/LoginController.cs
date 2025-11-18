using IdentityService.Features.Shared;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Features.Authantication.Commands.Login
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
        [HttpPost("Login")]
        public async Task<ActionResult<Result<AuthModel>>> Login([FromBody] LoginCommand login)
        {
            var response = await _mediator.Send(new LoginCommand(
                 login.Email,
                 login.Password
             ));
            await _unitOfWork.SaveChangesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
