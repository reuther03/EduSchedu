using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.Database;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;

public interface ISchoolUserRepository : IRepository<SchoolUser>
{
    Task<SchoolUser?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<Teacher?> GetTeacherByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default);
}