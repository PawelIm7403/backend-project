using Xunit;
using Infrastructure.Memory;
using CoreApp.Entities;

public class VehicleRepositoryTests
{
    private readonly InMemoryVehicleRepository _repository;

    public VehicleRepositoryTests()
    {
        _repository = new InMemoryVehicleRepository();
    }

    [Fact]
    public async Task FindByLicensePlateAsync_ShouldReturnVehicle_WhenExists()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = "KR2324S",
            Brand = "BMW",
            Color = "Black"
        };

        await _repository.AddAsync(vehicle);

        // Act
        var result = await _repository.FindByLicensePlateAsync("KR2324S");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("KR2324S", result!.LicensePlate);
        Assert.Equal("BMW", result.Brand);
        Assert.Equal("Black", result.Color);
    }

    [Fact]
    public async Task FindByLicensePlateAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _repository.FindByLicensePlateAsync("XYZ999");

        // Assert
        Assert.Null(result);
    }
}