using Application.Features.Identity.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Validators
{
	public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand> 
	{
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Request)
                .SetValidator(new RegisterUserRequestValidator());
        }
    }
}
