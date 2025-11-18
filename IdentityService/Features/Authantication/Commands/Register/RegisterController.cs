using IdentityService.Features.Shared;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Features.Authantication.Commands.Register
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

        [HttpPost("Register")]
        public async Task<ActionResult<Result<string>>> Register([FromBody] RegisterDTO regiserDTO)
        {
            var response = await _mediator.Send(new RegisterCommand(
                 regiserDTO.Email,
                 regiserDTO.Password,
                 regiserDTO.PhoneNumber
             ));
            await _unitOfWork.SaveChangesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public string GetHello()
        {
            
            return "hello youssef";
        }

    }
}
