using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
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
        => _dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Teacher?> GetTeacherByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => await _dbContext.Teachers.OfType<Teacher>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default)
        => await _dbContext.Teachers.AnyAsync(x => x.Id == id, cancellationToken);
}