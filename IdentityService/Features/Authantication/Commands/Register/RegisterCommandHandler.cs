using IdentityService.Features.Shared;
using IdentityService.Features.Shared.Queries.CheckExist;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Enums;
using IdentityService.Shared.Interfaces;
using MediatR;
using System.Security.Claims;

namespace IdentityService.Features.Authantication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IRepository<User> userRepository, IMediator mediator, IAuthService authService)
        {
            _userRepository = userRepository;
            _mediator = mediator;
            _authService = authService;
        }
        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var emailResult = await _mediator.Send(new CheckExistQuery<User>(e => e.Email == request.Email), cancellationToken);
            if (emailResult.Success)
                return Result<string>.FailResponse("Email already taken", errors: ["Email already taken"], 409);

            var phoneResult = await _mediator.Send(new CheckExistQuery<User>(e => e.PhoneNumber == request.PhoneNumber), cancellationToken);
            if (phoneResult.Success)
                return Result<string>.FailResponse("phone number already exist", errors: ["phone number already exist"], 409);

            var hashedPassword = await _authService.GeneratePasswordHashAsync(request.Password);

            var user = new User
            {
                Email = request.Email,
                HashPassword = hashedPassword,
                PhoneNumber = request.PhoneNumber
            };

            user.UserRoles.Add(new UserRole { RoleId = RoleType.Trainee });
            await _userRepository.AddAsync(user);

            return Result<string>.SuccessResponse(user.Email, "Email created successfully", 201);
        }
    }
}
