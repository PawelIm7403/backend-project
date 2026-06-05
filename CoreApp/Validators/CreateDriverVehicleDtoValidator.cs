using CoreApp.Dto;
using FluentValidation;

namespace CoreApp.Validators;

public class CreateDriverVehicleDtoValidator
    : AbstractValidator<CreateDriverVehicleDto>
{
    public CreateDriverVehicleDtoValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty()
            .MaximumLength(12);

        RuleFor(x => x.Brand)
            .NotEmpty()
            .MaximumLength(50);
    }
}