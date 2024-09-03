using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

public class SchoolRepository : ISchoolRepository
{
    private readonly DbSet<School> _schools;

    public SchoolRepository(ISchoolsDbContext dbContext)
    {
        _schools = dbContext.Schools;
    }

    public async Task<School?> GetByIdAsync(SchoolId id, CancellationToken cancellationToken = default)
        => await _schools.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(School school, CancellationToken cancellationToken = default)
        => await _schools.AddAsync(school, cancellationToken);

    public void Remove(School entity)
        => _schools.Remove(entity);
}