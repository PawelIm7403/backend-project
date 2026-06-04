namespace CoreApp.Dto;

public record RefreshTokenDto(
    string AccessToken,
    string RefreshToken
);