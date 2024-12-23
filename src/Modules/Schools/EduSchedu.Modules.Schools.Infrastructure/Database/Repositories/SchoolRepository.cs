using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
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
        => await _dbContext.Schools
            .Include(x => x.Classes)
            .ThenInclude(x => x.Lessons)
            .Include(x => x.Classes)
            .ThenInclude(x => x.LanguageProficiency)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<School>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Schools.ToListAsync(cancellationToken);

    // public Task<School?> GetByHeadmasterIdAsync(SchoolUserId headmasterId, CancellationToken cancellationToken = default)
    //     => _dbContext.Schools.FirstOrDefaultAsync(x => x.HeadmasterId == headmasterId, cancellationToken);

    #region Class

    public async Task<Class?> GetClassByIdAsync(SchoolId schoolId, ClassId classId, CancellationToken cancellationToken = default)
        => await _dbContext.Schools
            .Include(x => x.Classes)
            .ThenInclude(x => x.Lessons)
            .Where(x => x.Id == schoolId)
            .SelectMany(x => x.Classes)
            .FirstOrDefaultAsync(x => x.Id == classId, cancellationToken);

    //plan: usunac niepotrzebne dbsety
    public async Task AddClassAsync(Class @class, CancellationToken cancellationToken = default)
        => await _dbContext.Set<Class>().AddAsync(@class, cancellationToken);

    #endregion

    #region Lesson

    public async Task AddLessonAsync(Lesson lesson, CancellationToken cancellationToken = default)
        => await _dbContext.Lessons.AddAsync(lesson, cancellationToken);

    #endregion

    #region School service methods

    public async Task<List<UserId>> GetSchoolTeachersAsync(SchoolId schoolId, CancellationToken cancellationToken = default)
        => await _dbContext.Schools
            .Include(x => x.TeacherIds)
            .Where(x => x.Id == schoolId)
            .SelectMany(x => x.TeacherIds)
            //bez asnotracking byl blad
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<bool> IsHeadmasterAsync(UserId userId, SchoolId schoolId, CancellationToken cancellationToken = default)
        => _dbContext.Schools.AnyAsync(x => x.Id == schoolId && x.HeadmasterId == userId, cancellationToken);

    #endregion
}