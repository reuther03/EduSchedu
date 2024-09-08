using EduSchedu.Modules.Schools.Application.Abstractions.Database;
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

    public Task<bool> IsHeadmasterAsync(SchoolId schoolId, Guid userId, CancellationToken cancellationToken = default)
        => _dbContext.Schools.AnyAsync(x => x.Id == schoolId && x.HeadmasterId.Value == userId, cancellationToken);
}