using FluentValidation;

namespace IdentityService.Features.Authantication.Commands.Login
{
    public class LoginCommandValidator: AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator() {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6)
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number");
        }
    }
}
