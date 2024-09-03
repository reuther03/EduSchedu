using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

public class SchoolRepository : ISchoolRepository
{
    private readonly DbSet<School> _schools;

    public SchoolRepository(ISchoolsDbContext dbContext)
    {
        _schools = dbContext.Schools;
    }

    public async Task<School?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _schools.FirstOrDefaultAsync(x => x.Id.Value == id, cancellationToken);

    public async Task AddAsync(School entity, CancellationToken cancellationToken = default)
        => await _schools.AddAsync(entity, cancellationToken);

    public void Remove(School entity)
        => _schools.Remove(entity);
}