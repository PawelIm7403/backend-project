using System.Net;
using System.Net.Http.Json;
using CoreApp.Dto;
using WebApi;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnitTest;

public class AuthIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ShouldReturnTokens_WhenCredentialsAreCorrect()
    {
        // Przygotowanie danych logowania istniejacego administratora z seedera.
        var dto = new LoginDto
        {
            Email = "admin@parking.local",
            Password = "Admin@123!"
        };

        // Wywolanie endpointu logowania.
        var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

        // Sprawdzenie czy logowanie sie powiodlo.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>(options);

        // Sprawdzenie czy API zwrocilo tokeny i poprawne dane uzytkownika.
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result!.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
        Assert.Equal("admin@parking.local", result.User.Email);
    }
}
