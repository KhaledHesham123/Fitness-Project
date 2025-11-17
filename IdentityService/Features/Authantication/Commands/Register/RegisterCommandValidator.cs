using FluentValidation;

namespace IdentityService.Features.Authantication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {


            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress()
                .WithMessage("Invalid email address");

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number");

            RuleFor(x => x.PhoneNumber)
                 .NotEmpty()
                 .Matches(@"^(?:\+201[0-2,5][0-9]{8}|01[0-2,5][0-9]{8})$")
                 .WithMessage("Invalid phone number format");
        }
    }
}
