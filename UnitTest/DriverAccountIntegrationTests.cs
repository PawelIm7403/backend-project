using System.Net;
using System.Net.Http.Json;
using CoreApp.Dto;

namespace UnitTest;

public class DriverAccountIntegrationTests
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DriverAccountIntegrationTests(
        TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TopUpAccount_ShouldIncreaseBalance()
    {
        // Logowanie kierowcy, ktory bedzie doladowywal konto.
        await IntegrationTestHelper.LoginAsDriver(_client);

        // Doladowanie konta kwota 50.
        var topUpResponse =
            await _client.PostAsJsonAsync("/api/drivers/account/topup", new TopUpAccountDto(50m));

        Assert.Equal(HttpStatusCode.OK, topUpResponse.StatusCode);

        // Pobranie aktualnego stanu konta.
        var accountResponse =
            await _client.GetAsync("/api/drivers/account");

        Assert.Equal(HttpStatusCode.OK, accountResponse.StatusCode);

        var result =
            await accountResponse.Content.ReadFromJsonAsync<DriverAccountDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(result);
        // Saldo powinno byc rowne wykonanej wplacie.
        Assert.Equal(50m, result!.Balance);
    }

    [Fact]
    public async Task TopUpAccount_ShouldReturn400_WhenAmountIsZero()
    {
        // Logowanie kierowcy i proba doladowania niepoprawna kwota.
        await IntegrationTestHelper.LoginAsDriver(_client);

        var response =
            await _client.PostAsJsonAsync("/api/drivers/account/topup", new TopUpAccountDto(0m));

        // Kwota 0 powinna zostac odrzucona jako blad biznesowy.
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
