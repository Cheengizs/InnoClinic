using FluentValidation;
using Presentation.ViewModels;

namespace Presentation.Validators;

public class OfficeUpdateValidator : AbstractValidator<OfficeUpdateRequest>
{
    public OfficeUpdateValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Office ID is required.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City name is too long.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(150).WithMessage("Street name is too long.");

        RuleFor(x => x.HouseNumber)
            .NotEmpty().WithMessage("House number is required.");

        RuleFor(x => x.OfficeNumber)
            .NotEmpty().WithMessage("Office number is required.");

        RuleFor(x => x.RegistryPhoneNumber)
            .NotEmpty().WithMessage("Registry phone number is required.");
    }
}
