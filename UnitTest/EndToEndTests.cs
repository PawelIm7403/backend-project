using System.Net;
using System.Net.Http.Json;
using CoreApp.Dto;

namespace UnitTest;

public class AnonymousDriverEndToEndTests
{
    [Fact]
    public async Task AnonymousDriver_ShouldCheckCurrentSessionHistoryAndPay()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        // Krok 1: anonimowy kierowca sprawdza aktualna sesje po tablicy.
        var currentSessionResponse =
            await client.GetAsync("/api/public/vehicles/KR12345/current-session");

        Assert.Equal(HttpStatusCode.OK, currentSessionResponse.StatusCode);

        var currentSession =
            await currentSessionResponse.Content.ReadFromJsonAsync<ParkingSessionHistoryDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(currentSession);
        Assert.True(currentSession!.IsActive);

        // Krok 2: anonimowy kierowca sprawdza historie sesji pojazdu.
        var historyResponse =
            await client.GetAsync("/api/public/vehicles/KR12345/sessions");

        Assert.Equal(HttpStatusCode.OK, historyResponse.StatusCode);

        var history =
            await historyResponse.Content.ReadFromJsonAsync<List<ParkingSessionHistoryDto>>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(history);
        Assert.NotEmpty(history!);

        // Krok 3: anonimowy kierowca oplaca aktualna sesje bez konta.
        var paymentResponse =
            await client.PostAsync("/api/public/vehicles/KR12345/pay-current-session", null);

        Assert.Equal(HttpStatusCode.OK, paymentResponse.StatusCode);

        var payment =
            await paymentResponse.Content.ReadFromJsonAsync<PaymentResultDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(payment);
        Assert.Equal("KR12345", payment!.LicensePlate);
        Assert.Equal(10m, payment.Amount);
        Assert.True(payment.IsPaid);
    }
}

public class RegisteredDriverPaymentEndToEndTests
{
    [Fact]
    public async Task RegisteredDriver_ShouldLoginRegisterVehicleTopUpAndPayFromAccount()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        // Krok 1: kierowca loguje sie do systemu.
        await IntegrationTestHelper.LoginAsDriver(client);

        // Krok 2: kierowca rejestruje pojazd z aktywna sesja z seedera.
        await IntegrationTestHelper.RegisterVehicle(
            client,
            "KR12345",
            "BMW");

        // Krok 3: kierowca sprawdza aktualna sesje swojego pojazdu.
        var currentSessionResponse =
            await client.GetAsync("/api/drivers/vehicles/KR12345/current-session");

        Assert.Equal(HttpStatusCode.OK, currentSessionResponse.StatusCode);

        // Krok 4: kierowca doladowuje konto na przyszle sesje.
        var topUpResponse =
            await client.PostAsJsonAsync("/api/drivers/account/topup", new TopUpAccountDto(50m));

        Assert.Equal(HttpStatusCode.OK, topUpResponse.StatusCode);

        // Krok 5: kierowca placi za sesje z konta.
        var paymentResponse =
            await client.PostAsync("/api/drivers/vehicles/KR12345/pay-from-account", null);

        Assert.Equal(HttpStatusCode.OK, paymentResponse.StatusCode);

        var payment =
            await paymentResponse.Content.ReadFromJsonAsync<PaymentResultDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(payment);
        Assert.Equal(10m, payment!.Amount);
        Assert.True(payment.IsPaid);

        // Krok 6: saldo powinno byc pomniejszone o kwote platnosci.
        var accountResponse =
            await client.GetAsync("/api/drivers/account");

        Assert.Equal(HttpStatusCode.OK, accountResponse.StatusCode);

        var account =
            await accountResponse.Content.ReadFromJsonAsync<DriverAccountDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(account);
        Assert.Equal(40m, account!.Balance);
    }
}

public class RegisteredDriverRegistrationBonusEndToEndTests
{
    [Fact]
    public async Task RegisteredDriver_ShouldActivateRegistrationBonusAndPayZeroForShortSession()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        // Krok 1: kierowca loguje sie do systemu.
        await IntegrationTestHelper.LoginAsDriver(client);

        // Krok 2: kierowca rejestruje pojazd z aktywna sesja krotsza niz 60 minut.
        await IntegrationTestHelper.RegisterVehicle(
            client,
            "KR12345",
            "BMW");

        // Krok 3: kierowca aktywuje rabat za rejestracje.
        var discountResponse =
            await client.PostAsync("/api/drivers/discounts/activate-registration", null);

        Assert.Equal(HttpStatusCode.OK, discountResponse.StatusCode);

        // Krok 4: kierowca placi za sesje, ktora miesci sie w darmowym czasie.
        var paymentResponse =
            await client.PostAsync("/api/drivers/vehicles/KR12345/pay-from-account", null);

        Assert.Equal(HttpStatusCode.OK, paymentResponse.StatusCode);

        var payment =
            await paymentResponse.Content.ReadFromJsonAsync<PaymentResultDto>(
                IntegrationTestHelper.JsonOptions);

        Assert.NotNull(payment);
        Assert.Equal(0m, payment!.Amount);
        Assert.True(payment.IsPaid);

        // Krok 5: ponowna aktywacja tego samego rabatu powinna zwrocic blad biznesowy.
        var duplicateDiscountResponse =
            await client.PostAsync("/api/drivers/discounts/activate-registration", null);

        Assert.Equal(HttpStatusCode.BadRequest, duplicateDiscountResponse.StatusCode);
    }
}
