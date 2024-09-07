using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Kernel.Database;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;

public interface ISchoolRepository
{
    Task<School?> GetByIdAsync(SchoolId id, CancellationToken cancellationToken = default);
    Task<bool> IsHeadmasterAsync(SchoolId schoolId, Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(School school, CancellationToken cancellationToken = default);
}