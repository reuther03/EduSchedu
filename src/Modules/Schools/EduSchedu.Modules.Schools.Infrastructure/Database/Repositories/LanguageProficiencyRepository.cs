using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

internal class LanguageProficiencyRepository : Repository<LanguageProficiency, SchoolsDbContext>, ILanguageProficiencyRepository
{
    private readonly SchoolsDbContext _dbContext;

    public LanguageProficiencyRepository(SchoolsDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<LanguageProficiency?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.LanguageProficiencies.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}