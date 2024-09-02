using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Teacher : SchoolUser
{
    private readonly List<LanguageProficiencyId> _languageProficiencyIds = [];


    public IReadOnlyList<LanguageProficiencyId> LanguageProficiencyIds => _languageProficiencyIds.AsReadOnly();

    private Teacher()
    {
    }

    private Teacher(UserId id, Email email, Name fullName, Role role)
        : base(id, email, fullName, role)
    {
    }

    public static Teacher Create(UserId id, Email email, Name fullName, Role role)
    {
        if (role == Role.BackOffice)
            throw new DomainException("Role is invalid.");

        var teacher = new Teacher(id, email, fullName, role);
        return teacher;
    }


    public void AddLanguageProficiency(Guid languageProficiencyId)
    {
        if (!_languageProficiencyIds.Contains(languageProficiencyId))
        {
            _languageProficiencyIds.Add(languageProficiencyId);
        }
    }
}