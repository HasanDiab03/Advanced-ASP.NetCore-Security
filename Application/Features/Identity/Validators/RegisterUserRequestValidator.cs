using Common.Requests.Identity;
using FluentValidation;

namespace Application.Features.Identity.Validators
{
	public class RegisterUserRequestValidator : AbstractValidator<RegisterRequest>
	{
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.ConfirmPassword)
                .Must((req, confirm) => req.Password == confirm)
                .WithMessage("Password and confirm password don't match");
        }
    }
}
