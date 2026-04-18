using CoreApp.Dto;
using CoreApp.Enums;
using FluentValidation;

namespace CoreApp.Validators;

public class ParkingGateValidator : AbstractValidator<CreateGateDto>
{
    public ParkingGateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nazwa bramik jest wymagana.")
            .MaximumLength(20).WithMessage("Nazwa nie może przekraczać 20 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwa zawiera niedozwolone znaki.");
        
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Lokalizacja jest wymagana")
            .MaximumLength(50).WithMessage("Maksymalna ilość znaków to 50")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwa zawiera niedozwolone znaki.");
        
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Typ bramki jest niepoprawny.");
    }
}