using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

internal class SchoolRepository : Repository<School, SchoolsDbContext>, ISchoolRepository
{
    private readonly SchoolsDbContext _dbContext;

    public SchoolRepository(SchoolsDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<School?> GetByIdAsync(SchoolId id, CancellationToken cancellationToken = default)
        => await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    #region MyRegion

    public async Task<Class?> GetClassByIdAsync(SchoolId schoolId, ClassId classId, CancellationToken cancellationToken = default)
        => await _dbContext.Schools
            .Include(x => x.Classes)
            .ThenInclude(x => x.Lessons)
            .Where(x => x.Id == schoolId)
            .SelectMany(x => x.Classes)
            .FirstOrDefaultAsync(x => x.Id == classId, cancellationToken);

    public async Task AddClassAsync(Class @class, CancellationToken cancellationToken = default)
        => await _dbContext.Classes.AddAsync(@class, cancellationToken);

    #endregion

    #region Lesson

    public Task<List<Lesson>> GetLessonsByClassIdAsync(ClassId classId, CancellationToken cancellationToken = default)
        => _dbContext.Classes
            .Where(x => x.Id == classId)
            .SelectMany(x => x.Lessons)
            .ToListAsync(cancellationToken);

    public async Task<Lesson?> GetLessonByIdAsync(Guid lessonId, CancellationToken cancellationToken = default)
        => await _dbContext.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId, cancellationToken);

    public async Task AddLessonAsync(Lesson lesson, CancellationToken cancellationToken = default)
        => await _dbContext.Lessons.AddAsync(lesson, cancellationToken);

    #endregion
}