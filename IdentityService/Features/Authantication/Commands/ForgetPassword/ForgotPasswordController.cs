
using IdentityService.Features.Shared;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Features.Authantication.ForgotPassword
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IMediator mediator, IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<Result<ForgotPasswordDTO>>> ForgetPassword(ForgotPasswordCommand emailUser)
        {
            var response = await _mediator.Send(emailUser);
            await _unitOfWork.SaveChangesAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
