using System.Net;
using System.Net.Http.Json;
using CoreApp.Dto;

namespace UnitTest;

public class DriverVehicleIntegrationTests
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DriverVehicleIntegrationTests(
        TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterVehicle_ShouldAddVehicleToDriverVehicleList()
    {
        // Logowanie kierowcy wymagane do endpointow /api/drivers.
        await IntegrationTestHelper.LoginAsDriver(_client);

        // Losowa tablica zabezpiecza test przed konfliktem z innymi testami.
        var licensePlate = IntegrationTestHelper.NewLicensePlate();

        // Rejestracja nowego pojazdu kierowcy.
        await IntegrationTestHelper.RegisterVehicle(
            _client,
            licensePlate,
            "Toyota");

        // Pobranie listy pojazdow zalogowanego kierowcy.
        var response =
            await _client.GetAsync("/api/drivers/vehicles");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<List<DriverVehicleDto>>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(result);
        // Na liscie powinien pojawic sie przed chwila zarejestrowany pojazd.
        Assert.Contains(
            result!,
            vehicle => vehicle.LicensePlate == licensePlate &&
                       vehicle.Brand == "Toyota");
    }
}
