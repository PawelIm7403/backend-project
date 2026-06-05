# Parking Management API

## Opis projektu

Projekt został wykonany w technologii ASP.NET Core z wykorzystaniem wzorca Clean Architecture. Aplikacja umożliwia zarządzanie parkingiem, bramami wjazdowymi i wyjazdowymi, sesjami parkingowymi, przechwyceniami z kamer oraz obsługą kierowców.

System wykorzystuje uwierzytelnianie JWT, Identity, Entity Framework Core oraz bazę danych SQLite.

---

# Architektura projektu

Projekt został podzielony na następujące moduły:

## CoreApp

Warstwa domenowa zawierająca:

* encje
* DTO
* interfejsy repozytoriów
* interfejsy serwisów
* walidatory
* mapery
* logikę biznesową

## Infrastructure

Warstwa infrastrukturalna zawierająca:

* Entity Framework Core
* SQLite
* Identity
* JWT Authentication
* repozytoria EF
* konfigurację DI
* seedery danych

## WebApi

Warstwa prezentacji udostępniająca REST API.

Zawiera:

* kontrolery
* konfigurację JWT
* obsługę wyjątków
* konfigurację aplikacji

## UnitTest

Projekt testowy zawierający testy jednostkowe i integracyjne.

---

# Wykorzystane technologie

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* ASP.NET Identity
* JWT Authentication
* xUnit
* Swagger / OpenAPI

---

# Uwierzytelnianie

System wykorzystuje JWT Authentication.

Obsługiwane funkcjonalności:

* logowanie użytkownika
* generowanie Access Token
* generowanie Refresh Token
* odświeżanie tokenów
* unieważnianie tokenów
* role użytkowników
* polityki autoryzacyjne

---

# Role użytkowników

System wykorzystuje role Identity.

Przykładowa rola:

* Administrator

Administrator posiada pełny dostęp do systemu.

---

# Funkcjonalności parkingu

## Bramy parkingowe

* dodawanie bram
* pobieranie listy bram
* pobieranie szczegółów bramy
* aktualizacja statusu działania bramy

## Sesje parkingowe

* rejestrowanie sesji parkingowych
* śledzenie aktywnych sesji
* historia sesji

## Przechwycenia z kamer

* dodawanie przechwyceń
* pobieranie przechwyceń
* usuwanie przechwyceń

Usuwanie możliwe jest wyłącznie przez właściciela danych lub administratora.

---

# Funkcjonalności kierowców

## Kierowcy anonimowi

Anonimowy kierowca może:

* sprawdzić historię sesji pojazdu na podstawie numeru rejestracyjnego
* sprawdzić aktualną sesję pojazdu
* opłacić aktualną sesję parkingową

## Kierowcy zalogowani

Zalogowany kierowca może:

* zarejestrować pojazd
* przeglądać swoje pojazdy
* przeglądać historię sesji swoich pojazdów
* sprawdzić aktualną sesję pojazdu
* zasilić konto kierowcy
* opłacić sesję z wykorzystaniem środków na koncie

---

# System rabatów

System obsługuje dwa rodzaje rabatów.

## Registration Bonus

* aktywowany ręcznie przez kierowcę
* ważny przez 30 dni
* zapewnia dodatkowy darmowy czas parkowania

## Loyalty Discount

* aktywowany ręcznie przez kierowcę
* dostępny po osiągnięciu minimum 100 sesji parkingowych
* zapewnia obniżenie kosztu parkowania

---

# Baza danych

Projekt wykorzystuje SQLite.

Migracje Entity Framework:

* InitialCreate
* AddRefreshTokens
* AddCameraCaptureOwner
* AddParkingSessionPaymentStatus
* AddDriverVehicles
* AddDriverAccounts
* AddDriverDiscounts

---

# Dane inicjalizacyjne

Aplikacja wykorzystuje mechanizm Seederów.

Podczas uruchomienia dodawane są przykładowe:

* role
* użytkownicy
* bramy parkingowe
* taryfy parkingowe
* przykładowe sesje parkingowe

---

# Uruchomienie projektu

## Migracje

W katalogu Infrastructure:

```bash
dotnet ef database update --startup-project ../WebApi
```

## Uruchomienie aplikacji

W katalogu WebApi:

```bash
dotnet run
```

# Autor

Paweł Imiołek

github: https://github.com/PawelIm7403/backend-project
