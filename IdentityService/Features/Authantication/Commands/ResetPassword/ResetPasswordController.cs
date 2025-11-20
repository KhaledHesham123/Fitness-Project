
using IdentityService.Features.Authantication.ForgotPassword;
using IdentityService.Features.Shared;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Features.Authantication.ResetPassword
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IMediator mediator, IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<Result<string>>> ResetPassword(ResetPasswordCommand resetedPassword)
        {
            var response = await _mediator.Send(resetedPassword);
            await _unitOfWork.SaveChangesAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
