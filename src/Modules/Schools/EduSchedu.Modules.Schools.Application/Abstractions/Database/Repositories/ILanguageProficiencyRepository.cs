using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.Database;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;

public interface ILanguageProficiencyRepository : IRepository<LanguageProficiency>
{
    Task<LanguageProficiency?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}