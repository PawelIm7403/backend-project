using CoreApp.Dto;
using FluentValidation;

namespace CoreApp.Validators;

public class CreateCameraCaptureValidator : AbstractValidator<CreateCameraCaptureDto>
{
    public CreateCameraCaptureValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("Numer rejestracyjny jest wymagany.")
            .MaximumLength(12).WithMessage("Numer rejestracyjny nie może przekraczać 12 znaków.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Marka pojazdu jest wymagana.")
            .MaximumLength(30).WithMessage("Marka pojazdu nie może przekraczać 30 znaków.");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Kolor pojazdu jest wymagany.")
            .MaximumLength(20).WithMessage("Kolor pojazdu nie może przekraczać 20 znaków.");

        RuleFor(x => x.ImagePath)
            .MaximumLength(255).WithMessage("Ścieżka obrazu nie może przekraczać 255 znaków.")
            .When(x => !string.IsNullOrWhiteSpace(x.ImagePath));
    }
}