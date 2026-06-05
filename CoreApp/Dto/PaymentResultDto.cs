namespace CoreApp.Dto;

public record PaymentResultDto(
    Guid SessionId,
    string LicensePlate,
    decimal Amount,
    bool IsPaid,
    DateTime PaidAt
);