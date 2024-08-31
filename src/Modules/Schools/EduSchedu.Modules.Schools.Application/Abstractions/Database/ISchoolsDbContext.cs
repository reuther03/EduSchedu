using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database;

public interface ISchoolsDbContext
{
    DbSet<Class> Classes { get; }
    DbSet<School> Schools { get; }
    DbSet<Teacher> Teachers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}