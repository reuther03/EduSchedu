namespace EduSchedu.Shared.Abstractions.Services;

public interface IModuleSeeder
{
    Task SeedAsync(CancellationToken cancellationToken);
}