using Exam_System.Domain.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Authantication.CreateUserToken
{
    public class CreateUserTokenCommandHandler : IRequestHandler<CreateUserTokenCommand>
    {
        private readonly IRepository<UserToken> _userTokenRepository;

        public CreateUserTokenCommandHandler(IRepository<UserToken> userRepository)
        {
            _userTokenRepository = userRepository;
        }
        public async Task<Unit> Handle(CreateUserTokenCommand request, CancellationToken cancellationToken)
        {
            var userToken = await _userTokenRepository.FirstOrDefaultAsync(r => r.UserId == request.UserId);
            if (userToken == null)
            {
                await _userTokenRepository.AddAsync(new UserToken
                {
                    UserId = request.UserId,
                    Token = request.Token,
                    CreatedAt = DateTime.UtcNow,
                    ExpiredDate = DateTime.UtcNow.AddMinutes(request.expiredInMin),
                });
            }
            else
            {
                userToken.Token = request.Token;
                userToken.CreatedAt = DateTime.UtcNow;
                userToken.ExpiredDate = DateTime.UtcNow.AddMinutes(request.expiredInMin);
            }
            return Unit.Value;
        }
    }
}
