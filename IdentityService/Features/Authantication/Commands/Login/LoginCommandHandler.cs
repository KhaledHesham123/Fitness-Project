using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Authantication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthModel>>
    {
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly IRepository<User> _userRepository;
        public LoginCommandHandler(IAuthService authService, IMediator mediator, IRepository<User> userRepository)
        {
            _authService = authService;
            _mediator = mediator;
            _userRepository = userRepository;
        }
        public async Task<Result<AuthModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null)
                return Result<AuthModel>.FailResponse("email or password incorect", errors: ["incorect email or password"], statusCode: 401);

            var passwordCheck = await _authService.CheckPasswordAsync(user, request.Password);
            if (!passwordCheck)
                return Result<AuthModel>.FailResponse("email or password incorect", errors: ["incorect email or password"], statusCode: 401);

            var tokens = await _authService.GenerateTokensAsync(user);

            return Result<AuthModel>.SuccessResponse(tokens, "login successful");

        }
    }
}
