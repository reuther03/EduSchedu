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

    public Task<Class?> GetClassByIdAsync(ClassId id, CancellationToken cancellationToken = default)
        => _dbContext.Classes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddClassAsync(Class @class, CancellationToken cancellationToken = default)
        => await _dbContext.Classes.AddAsync(@class, cancellationToken);

    #endregion
}