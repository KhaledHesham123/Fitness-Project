using FluentValidation;

namespace  IdentityService.Features.Authantication.ResetPassword

{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.PasswordToken)
                .NotEmpty()
                .Length(6).WithMessage("Password must be 6 characters length.");

            RuleFor(x => x.NewPassword)
              .MinimumLength(6)
              .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
              .Matches("[0-9]").WithMessage("Password must contain a number");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
