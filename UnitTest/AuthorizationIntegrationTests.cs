using System.Net;
using WebApi;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreApp.Dto;

namespace UnitTest;

public class AuthorizationIntegrationTests
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthorizationIntegrationTests(
        TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAccount_ShouldReturn401_WhenTokenIsMissing()
    {
        // Proba wejscia na endpoint kierowcy bez tokenu JWT.
        var response =
            await _client.GetAsync("/api/drivers/account");

        // Endpoint powinien odrzucic uzytkownika niezalogowanego.
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
    
    
}
