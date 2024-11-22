using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Abstractions.Services;

public class SchoolService : ISchoolService
{
    private readonly ISchoolRepository _schoolRepository;

    public SchoolService(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }

    public async Task<List<UserId>> GetSchoolTeachersAsync(SchoolId schoolId, CancellationToken cancellationToken)
        => await _schoolRepository.GetSchoolTeachersAsync(schoolId, cancellationToken);

    public Task<bool> IsHeadmasterAsync(UserId userId, SchoolId schoolId, CancellationToken cancellationToken)
        => _schoolRepository.IsHeadmasterAsync(userId, schoolId, cancellationToken);
}