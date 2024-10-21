using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Teacher : SchoolUser
{
    private readonly List<LanguageProficiencyId> _languageProficiencyIds = [];

    public Schedule Schedule { get; private set; }
    public bool IsHeadmaster => Role == Role.HeadMaster;
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
        if (role is Role.BackOffice or Role.Student)
            throw new DomainException("Role is invalid.");

        var teacher = new Teacher(id, email, fullName, role);
        return teacher;
    }


    public void AddLanguageProficiency(Guid languageProficiencyId)
    {
        if (_languageProficiencyIds.Contains(languageProficiencyId))
        {
            throw new DomainException("Language proficiency already exists");
        }

        _languageProficiencyIds.Add(languageProficiencyId);
    }

    public void RemoveLanguageProficiency(Guid languageProficiencyId)
    {
        if (!_languageProficiencyIds.Contains(languageProficiencyId))
        {
            throw new DomainException("Language proficiency does not exist");
        }

        _languageProficiencyIds.Remove(languageProficiencyId);
    }

    public void SetSchedule(Schedule schedule)
        => Schedule = schedule;
}