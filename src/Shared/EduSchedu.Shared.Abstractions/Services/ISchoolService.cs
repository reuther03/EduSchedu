﻿using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Shared.Abstractions.Services;

public interface ISchoolService
{
    Task<List<UserId>> GetSchoolTeachersAsync(SchoolId schoolId, CancellationToken cancellationToken);

    Task<bool> IsHeadmasterAsync(UserId userId, SchoolId schoolId, CancellationToken cancellationToken);
}