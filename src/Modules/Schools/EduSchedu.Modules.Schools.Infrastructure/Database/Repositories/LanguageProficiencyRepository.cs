using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Repositories;

public class LanguageProficiencyRepository : ILanguageProficiencyRepository
{
    public Task<LanguageProficiency?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}