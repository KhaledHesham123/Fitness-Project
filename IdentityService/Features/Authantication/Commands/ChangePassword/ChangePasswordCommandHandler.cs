
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Authantication.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IAuthService _authService;

        public ChangePasswordCommandHandler(IRepository<User> userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var hashedPassword = await _authService.GeneratePasswordHashAsync(request.NewPassword);
            await _userRepository
                .GetAll()
                .Where(u => u.Id == request.UserId)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.HashPassword, hashedPassword));
            return Unit.Value;
        }
    }
}
