using FluentValidation;

namespace IdentityService.Features.Authantication.ForgotPassword
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress()
                .WithMessage("Invalid email address");
        }
    }
}
