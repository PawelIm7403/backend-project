using System.Net;
using System.Net.Http.Json;
using CoreApp.Dto;

namespace UnitTest;

public class DriverPaymentFromAccountIntegrationTests
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DriverPaymentFromAccountIntegrationTests(
        TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PayFromAccount_ShouldDeductMoneyFromBalance()
    {
        // Kierowca loguje sie i przypisuje do siebie pojazd z aktywna sesja z seedera.
        await IntegrationTestHelper.LoginAsDriver(_client);

        await IntegrationTestHelper.RegisterVehicle(
            _client,
            "KR12345",
            "BMW");

        // Doladowanie konta, aby bylo z czego pobrac oplate.
        await _client.PostAsJsonAsync("/api/drivers/account/topup", new TopUpAccountDto(50m));

        // Platnosc za aktualna sesje z konta kierowcy.
        var response =
            await _client.PostAsync("/api/drivers/vehicles/KR12345/pay-from-account", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payment =
            await response.Content.ReadFromJsonAsync<PaymentResultDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(payment);
        // Sesja kosztuje 10, wiec taka kwota powinna zostac pobrana.
        Assert.Equal(10m, payment!.Amount);
        Assert.True(payment.IsPaid);

        // Po platnosci saldo powinno spasc z 50 do 40.
        var accountResponse =
            await _client.GetAsync("/api/drivers/account");

        var account =
            await accountResponse.Content.ReadFromJsonAsync<DriverAccountDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(account);
        Assert.Equal(40m, account!.Balance);
    }
}

public class DriverPaymentWithRegistrationBonusIntegrationTests
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DriverPaymentWithRegistrationBonusIntegrationTests(
        TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task PayFromAccount_ShouldBeFree_WhenRegistrationBonusCoversSession()
    {
        // Kierowca loguje sie i przypisuje pojazd z krotka aktywna sesja.
        await IntegrationTestHelper.LoginAsDriver(_client);

        await IntegrationTestHelper.RegisterVehicle(
            _client,
            "KR12345",
            "BMW");

        // RegistrationBonus daje darmowe parkowanie dla sesji do 60 minut.
        await _client.PostAsync("/api/drivers/discounts/activate-registration", null);

        // Platnosc powinna zakonczyc sesje jako oplacona, ale bez pobrania salda.
        var response =
            await _client.PostAsync("/api/drivers/vehicles/KR12345/pay-from-account", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payment =
            await response.Content.ReadFromJsonAsync<PaymentResultDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(payment);
        // Kwota 0 potwierdza, ze rabat pokryl cala sesje.
        Assert.Equal(0m, payment!.Amount);
        Assert.True(payment.IsPaid);
    }
}
