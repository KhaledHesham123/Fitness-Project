using IdentityService.Features.Shared;
using IdentityService.Features.Shared.CheckExist;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using System.Security.Claims;

namespace IdentityService.Features.Authantication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMediator _mediator;

        public RegisterCommandHandler(IRepository<User> userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }
        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var emailResult = await _mediator.Send(new CheckExistQuery<User>(e => e.Email == request.Email), cancellationToken);
            if (emailResult.Success)
                return Result<string>.FailResponse("Email already taken");

            var phoneResult = await _mediator.Send(new CheckExistQuery<User>(e => e.PhoneNumber == request.PhoneNumber), cancellationToken);
            if (phoneResult.Success)
                return Result<string>.FailResponse("phone number already exist");

            var user = new User
            {
                Email = request.Email,
                HashPassword = request.Password,
                PhoneNumber = request.PhoneNumber
            };
            await _userRepository.AddAsync(user);

            //var Claims = new List<UserClaim>();
            //Claims.Add(new UserClaim(user, ClaimTypes.NameIdentifier, user.Id.ToString()));
            //Claims.Add(new UserClaim(user, ClaimTypes.Email, user.Email));
            //Claims.Add(new UserClaim(user, ClaimTypes.Role, "Student"));

            //await _userRepository.AddClaimsAsync(Claims);
            return Result<string>.SuccessResponse("Email created successfully");
        }
    }
}
