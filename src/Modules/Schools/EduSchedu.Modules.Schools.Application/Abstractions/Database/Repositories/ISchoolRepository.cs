using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Kernel.Database;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;

public interface ISchoolRepository : IRepository<School>
{
    Task<School?> GetByIdAsync(SchoolId id, CancellationToken cancellationToken = default);
    Task<List<School>> GetAllAsync(CancellationToken cancellationToken = default);

    #region @Class

    Task<Class?> GetClassByIdAsync(SchoolId schoolId, ClassId classId, CancellationToken cancellationToken = default);
    Task AddClassAsync(Class @class, CancellationToken cancellationToken = default);

    #endregion

    #region Lesson

    Task AddLessonAsync(Lesson lesson, CancellationToken cancellationToken = default);

    #endregion

    #region School service methods

    Task<List<UserId>> GetSchoolTeachersAsync(SchoolId schoolId, CancellationToken cancellationToken = default);
    Task<bool> IsHeadmasterAsync(UserId userId, SchoolId schoolId, CancellationToken cancellationToken = default);

    #endregion
}