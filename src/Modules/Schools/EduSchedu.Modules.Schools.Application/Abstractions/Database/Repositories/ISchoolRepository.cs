using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Kernel.Database;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;

public interface ISchoolRepository
{
    Task AddAsync(School school, CancellationToken cancellationToken = default);
    Task<School?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}