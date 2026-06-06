using System.Net;
using System.Net.Http.Json;
using CoreApp.Dto;

namespace UnitTest;

public class PublicVehicleIntegrationTests
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PublicVehicleIntegrationTests(
        TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPublicCurrentSession_ShouldReturnSession_WhenVehicleExists()
    {
        // Anonimowy kierowca pyta o aktualna sesje po numerze rejestracyjnym.
        var response =
            await _client.GetAsync("/api/public/vehicles/KR12345/current-session");

        // API powinno zwrocic aktywna sesje testowego pojazdu z seedera.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<ParkingSessionHistoryDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(result);
        Assert.True(result!.IsActive);
        Assert.Equal("KR12345", result.Vehicle.LicensePlate);
    }

    [Fact]
    public async Task GetPublicVehicleSessions_ShouldReturnHistory_WhenVehicleExists()
    {
        // Anonimowy kierowca pobiera historie sesji po tablicy rejestracyjnej.
        var response =
            await _client.GetAsync("/api/public/vehicles/KR12345/sessions");

        // Historia powinna istniec dla pojazdu przygotowanego przez seeder.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<List<ParkingSessionHistoryDto>>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(result);
        Assert.NotEmpty(result!);
    }
}
