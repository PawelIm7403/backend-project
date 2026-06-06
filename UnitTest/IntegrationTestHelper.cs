using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreApp.Dto;

namespace UnitTest;

public static class IntegrationTestHelper
{
    public static JsonSerializerOptions JsonOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task LoginAsDriver(HttpClient client)
    {
        var dto = new LoginDto
        {
            Email = "jan.kowalski@parking.local",
            Password = "User@123!"
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", dto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);

        Assert.NotNull(result);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result!.AccessToken);
    }

    public static async Task LoginAsAnna(HttpClient client)
    {
        var dto = new LoginDto
        {
            Email = "anna.nowak@parking.local",
            Password = "User@123!"
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", dto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);

        Assert.NotNull(result);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result!.AccessToken);
    }

    public static async Task RegisterVehicle(
        HttpClient client,
        string licensePlate,
        string brand)
    {
        var dto = new CreateDriverVehicleDto
        {
            LicensePlate = licensePlate,
            Brand = brand
        };

        var response =
            await client.PostAsJsonAsync("/api/drivers/vehicles", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    public static string NewLicensePlate()
    {
        return $"T{Guid.NewGuid():N}"[..8].ToUpperInvariant();
    }
}
