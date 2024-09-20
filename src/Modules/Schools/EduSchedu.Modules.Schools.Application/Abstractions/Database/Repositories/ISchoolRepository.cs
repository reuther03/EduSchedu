using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Kernel.Database;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;

public interface ISchoolRepository : IRepository<School>
{
    Task<School?> GetByIdAsync(SchoolId id, CancellationToken cancellationToken = default);
    Task<List<School>> GetAllAsync(CancellationToken cancellationToken = default);
    // Task<School?> GetByHeadmasterIdAsync(UserId headmasterId, CancellationToken cancellationToken = default);

    #region @Class

    Task<Class?> GetClassByIdAsync(SchoolId schoolId, ClassId classId, CancellationToken cancellationToken = default);
    Task AddClassAsync(Class @class, CancellationToken cancellationToken = default);

    #endregion

    #region Lesson

    Task<List<Lesson>> GetLessonsByClassIdAsync(ClassId classId, CancellationToken cancellationToken = default);
    Task<Lesson?> GetLessonByIdAsync(Guid lessonId, CancellationToken cancellationToken = default);
    Task AddLessonAsync(Lesson lesson, CancellationToken cancellationToken = default);

    #endregion
}