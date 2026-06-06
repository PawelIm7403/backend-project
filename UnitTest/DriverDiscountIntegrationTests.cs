using System.Net;
using System.Net.Http.Json;
using CoreApp.Dto;
using CoreApp.Enums;

namespace UnitTest;

public class DriverDiscountIntegrationTests
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DriverDiscountIntegrationTests(
        TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ActivateRegistrationBonus_ShouldCreateDiscount()
    {
        // Logowanie kierowcy, ktory nie ma jeszcze aktywnego rabatu.
        await IntegrationTestHelper.LoginAsDriver(_client);

        // Aktywacja rabatu za rejestracje.
        var response =
            await _client.PostAsync("/api/drivers/discounts/activate-registration", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result =
            await response.Content.ReadFromJsonAsync<DriverDiscountDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(result);
        // Rabat powinien byc aktywny i dawac 60 dodatkowych darmowych minut.
        Assert.Equal(DriverDiscountType.RegistrationBonus, result!.Type);
        Assert.True(result.IsActive);
        Assert.Equal(60, result.ExtraFreeMinutes);
    }

    [Fact]
    public async Task ActivateRegistrationBonus_ShouldReturn400_WhenDiscountIsAlreadyActive()
    {
        // Anna aktywuje rabat pierwszy raz.
        await IntegrationTestHelper.LoginAsAnna(_client);

        await _client.PostAsync("/api/drivers/discounts/activate-registration", null);

        // Druga aktywacja tego samego aktywnego rabatu powinna byc odrzucona.
        var response =
            await _client.PostAsync("/api/drivers/discounts/activate-registration", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ActivateLoyaltyDiscount_ShouldReturn400_WhenDriverHasLessThan100Sessions()
    {
        // Kierowca bez 100 sesji nie powinien dostac rabatu lojalnosciowego.
        await IntegrationTestHelper.LoginAsAnna(_client);

        var response =
            await _client.PostAsync("/api/drivers/discounts/activate-loyalty", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
