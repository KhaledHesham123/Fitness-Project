using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;

namespace IdentityService.Features.Authantication.Commands.AddRefreshToken
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand, Result<bool>>
    {
        private readonly IRepository<RefreshToken> _repository; 

        public AddRefreshTokenCommandHandler(IRepository<RefreshToken> repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(request.token);
            return Result<bool>.Response(true);
        }
    }
}
