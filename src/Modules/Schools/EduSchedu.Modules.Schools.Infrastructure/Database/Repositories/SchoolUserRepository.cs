using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

internal class SchoolUserRepository : ISchoolUserRepository
{
    private readonly SchoolsDbContext _dbContext;
    private readonly DbSet<SchoolUser> _teachers;

    public SchoolUserRepository(SchoolsDbContext dbContext)
    {
        _dbContext = dbContext;
        _teachers = dbContext.Teachers;
    }

    public async Task<SchoolUser?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => await _teachers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> ExistsAsync(UserId id, CancellationToken cancellationToken = default)
        => await _teachers.AnyAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(SchoolUser user, CancellationToken cancellationToken = default)
        => await _teachers.AddAsync(user, cancellationToken);

    public void Remove(SchoolUser entity)
        => _teachers.Remove(entity);
}