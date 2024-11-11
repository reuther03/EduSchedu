using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

internal class SchoolUserRepository : Repository<SchoolUser, SchoolsDbContext>, ISchoolUserRepository
{
    private readonly SchoolsDbContext _dbContext;

    public SchoolUserRepository(SchoolsDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<SchoolUser?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => _dbContext.SchoolUsers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<Teacher>> GetTeachersByIdsAsync(IEnumerable<UserId> ids, CancellationToken cancellationToken = default)
        => _dbContext.Teachers
            .Include(x => x.Schedule)
            .ThenInclude(x => x.ScheduleItems)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

    public Task<SchoolUser?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        => _dbContext.SchoolUsers.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task<Teacher?> GetTeacherByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => await _dbContext.Teachers
            .Include(x => x.Schedule)
            .ThenInclude(x => x.ScheduleItems)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default)
        => await _dbContext.SchoolUsers.AnyAsync(x => x.Id == id, cancellationToken);

    public Task<Schedule?> GetTeacherScheduleAsync(UserId teacherId, CancellationToken cancellationToken = default)
        => _dbContext.Schedules.FirstOrDefaultAsync(x => x.SchoolUserId == teacherId, cancellationToken);

    public Task<Student?> GetStudentByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}