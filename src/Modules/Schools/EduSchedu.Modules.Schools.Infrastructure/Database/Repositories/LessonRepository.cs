using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Infrastructure.Postgres;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

internal class LessonRepository : Repository<Lesson, SchoolsDbContext>, ILessonRepository
{
    public LessonRepository(SchoolsDbContext dbContext) : base(dbContext)
    {
    }
}