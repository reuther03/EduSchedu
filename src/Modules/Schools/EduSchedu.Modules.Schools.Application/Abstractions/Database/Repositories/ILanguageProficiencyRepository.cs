using EduSchedu.Modules.Schools.Domain.Schools;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;

public interface ILanguageProficiencyRepository
{
    Task<LanguageProficiency?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}