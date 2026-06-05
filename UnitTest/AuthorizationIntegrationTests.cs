using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreApp.Dto;

namespace UnitTest;

public class AuthorizationIntegrationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthorizationIntegrationTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAccount_ShouldReturn401_WhenTokenIsMissing()
    {
        var response =
            await _client.GetAsync("/api/drivers/account");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
    
    
}
