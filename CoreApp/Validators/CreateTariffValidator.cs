using CoreApp.Dto;
using FluentValidation;

namespace CoreApp.Validators;

public class CreateTariffValidator : AbstractValidator<CreateTariffDto>
{
    public CreateTariffValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nazwa taryfy jest wymagana.")
            .MaximumLength(50).WithMessage("Nazwa taryfy nie może przekraczać 50 znaków.");

        RuleFor(x => x.FreeMinutes)
            .GreaterThanOrEqualTo(0).WithMessage("Liczba darmowych minut nie może być ujemna.");

        RuleFor(x => x.HourlyRate)
            .GreaterThanOrEqualTo(0).WithMessage("Stawka godzinowa nie może być ujemna.");

        RuleFor(x => x.DailyMaxRate)
            .GreaterThanOrEqualTo(0).WithMessage("Maksymalna stawka dobowa nie może być ujemna.");

        RuleFor(x => x.DailyMaxRate)
            .GreaterThanOrEqualTo(x => x.HourlyRate)
            .WithMessage("Maksymalna stawka dobowa nie może być mniejsza niż stawka godzinowa.");
    }
}