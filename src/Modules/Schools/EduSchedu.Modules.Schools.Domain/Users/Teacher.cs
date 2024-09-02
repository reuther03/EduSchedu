using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Teacher : SchoolUser
{
    private readonly List<LanguageProficiencyId> _languageProficiencyIds = [];
    private readonly List<Lesson> _lessons = [];

    public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();
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

    public void AddLesson(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
    {
        if (_lessons.Any(x => x.Day == day && x.StartTime == startTime && x.EndTime == endTime))
        {
            throw new DomainException("Lesson already exists");
        }

        var lesson = Lesson.Create(day, startTime, endTime);
        _lessons.Add(lesson);
    }
}