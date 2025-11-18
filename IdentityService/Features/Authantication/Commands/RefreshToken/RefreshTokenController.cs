using IdentityService.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Features.Authantication.Commands.ExtendRefreshToken
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _jwtService;

        public AuthController(IAuthService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthModel>> Refresh(string refreshToken)
        {
            var tokenResult = await _jwtService.RefreshTokenAsync(refreshToken);
            if (!tokenResult.IsAuthenticated)
                return Unauthorized(tokenResult);
            return Ok(tokenResult);
        }
    }
}
