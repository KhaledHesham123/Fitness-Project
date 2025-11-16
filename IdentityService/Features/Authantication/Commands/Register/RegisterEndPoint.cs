using IdentityService.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Features.Authantication.Commands.Register
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Result<string>>> Register([FromBody] RegisterDTO regiserDTO)
        {
            var response = await _mediator.Send(new RegisterCommand(
                 regiserDTO.Email,
                 regiserDTO.Password,
                 regiserDTO.PhoneNumber
             ));
            return response.Success ? Ok(response) : BadRequest(response);

        }
    }
}
