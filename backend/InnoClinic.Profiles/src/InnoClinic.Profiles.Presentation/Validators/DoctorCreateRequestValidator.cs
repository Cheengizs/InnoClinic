using FluentValidation;
using InnoClinic.Profiles.Presentation.Contracts;

namespace InnoClinic.Profiles.Presentation.Validators;

public class DoctorCreateRequestValidator : AbstractValidator<DoctorCreateRequest>
{
    public DoctorCreateRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Please, enter the first name");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Please, enter the last name");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Please, select the date")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date fields are less or equal to current date");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Please, enter the email")
            .EmailAddress().WithMessage("You've entered an invalid email");

        RuleFor(x => x.SpecializationId)
            .NotEmpty().WithMessage("Please, choose the specialisation");

        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Please, choose the office");

        RuleFor(x => x.CareerStartYear)
            .NotEmpty().WithMessage("Please, select the year")
            .LessThanOrEqualTo(DateTime.Today.Year)
            .WithMessage("Date fields are less or equal to current date");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Please, choose the status");
    }
}
