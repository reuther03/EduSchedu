using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Infrastructure.Postgres;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

internal class ClassRepository : Repository<Class, SchoolsDbContext>, IClassRepository
{
    public ClassRepository(SchoolsDbContext dbContext) : base(dbContext)
    {
    }
}