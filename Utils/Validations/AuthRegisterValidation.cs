using FluentValidation;
using SmartAlertAPI.Models.Dto;
using System.Runtime.CompilerServices;

namespace SmartAlertAPI.Utils.Validations
{
    public class AuthRegisterValidation : AbstractValidator<UserSignupDto>
    {
        public AuthRegisterValidation()
        {
            RuleFor(model => model.FirstName).NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50)
                .Matches("^[A-Za-z]+$").WithMessage("First name must cotnain only letters of the latin alphabet");
            
            RuleFor(model => model.LastName).NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50)
                .Matches("^[A-Za-z]+$").WithMessage("Last name must cotnain only letters of the latin alphabet"); ;
            
            RuleFor(model => model.Email).NotEmpty().EmailAddress();
            
            RuleFor(model => model.Username).NotEmpty().MinimumLength(3).MaximumLength(25)
                .Matches("^[A-Z][A-Za-z0-9]+$")
                .WithMessage("Username must start with an uppercase letter, have at least a number and be over 3 letters length");
            
            RuleFor(model => model.PhoneNumber).NotEmpty().Length(10).Matches("^[0-9]+$");
            
            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                    .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
        }
    }
}
