using Domain.Shared;
using FluentValidation;
using Presentation.Contracts;

namespace Presentation.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .MaximumLength(Constants.MaxEmailLength).WithMessage("Email is too long")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(Constants.PasswordMinLength).WithMessage($"Password must be at least {Constants.PasswordMinLength} characters long")
            .MaximumLength(Constants.PasswordMaxLength).WithMessage($"Password must be no more than {Constants.PasswordMaxLength} characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(Constants.MaxNumberLength).WithMessage("Phone number is too long")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber)); 
    }
}
