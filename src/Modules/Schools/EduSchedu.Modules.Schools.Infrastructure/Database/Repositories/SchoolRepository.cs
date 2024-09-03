using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

internal class SchoolRepository : ISchoolRepository
{
    private readonly SchoolsDbContext _dbContext;
    private readonly DbSet<School> _schools;

    public SchoolRepository(SchoolsDbContext dbContext)
    {
        _dbContext = dbContext;
        _schools = dbContext.Schools;
    }

    public async Task<School?> GetByIdAsync(SchoolId id, CancellationToken cancellationToken = default)
        => await _schools.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(School school, CancellationToken cancellationToken = default)
    {
        _dbContext.Add(school);
    }

    public void Remove(School entity)
        => _schools.Remove(entity);
}