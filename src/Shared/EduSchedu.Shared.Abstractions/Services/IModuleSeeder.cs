using Microsoft.Extensions.Configuration;

namespace EduSchedu.Shared.Abstractions.Services;

public interface IModuleSeeder
{
    Task SeedAsync(IConfiguration configuration, CancellationToken cancellationToken);
}