using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Kernel.Database;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;

public interface ISchoolRepository : IRepository<School>
{
    Task<School?> GetByIdAsync(SchoolId id, CancellationToken cancellationToken = default);

    #region @Class

    Task<Class?> GetClassByIdAsync(ClassId id, CancellationToken cancellationToken = default);
    Task AddClassAsync(Class @class, CancellationToken cancellationToken = default);

    #endregion
}