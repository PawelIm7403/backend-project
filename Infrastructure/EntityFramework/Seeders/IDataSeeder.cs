namespace Infrastructure.EntityFramework.Seeders;

public interface IDataSeeder
{
    int Order { get; }
    Task SeedAsync();
}