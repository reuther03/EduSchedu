using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Teacher : Entity<UserId>
{
    private readonly List<LanguageProficiencyId> _languageProficiencyIds = [];

    public Email Email { get; private set; }
    public Name FullName { get; private set; }
    public Role Role { get; private set; }
    public SchoolId? SchoolId { get; private set; }
    public IReadOnlyList<LanguageProficiencyId> LanguageProficiencyIds => _languageProficiencyIds.AsReadOnly();

    private Teacher()
    {
    }

    private Teacher(UserId id, Email email, Name fullName, Role role)
        : base(id)
    {
        Email = email;
        FullName = fullName;
        Role = role;
    }

    public static Teacher Create(UserId id, Email email, Name fullName, Role role)
        => new Teacher(id, email, fullName, role);

    public void SetSchool(SchoolId schoolId)
        => SchoolId = schoolId;

    public void AddLanguageProficiency(Guid languageProficiencyId)
    {
        if (!_languageProficiencyIds.Contains(languageProficiencyId))
        {
            _languageProficiencyIds.Add(languageProficiencyId);
        }
    }
}