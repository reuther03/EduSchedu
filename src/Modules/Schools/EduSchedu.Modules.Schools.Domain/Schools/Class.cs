using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Class : Entity<ClassId>
{
    private readonly List<LanguageProficiencyId> _languageProficiencyIds = [];
    private readonly List<Lesson> _lessons = [];

    public Name Name { get; private set; }
    public IReadOnlyList<LanguageProficiencyId> LanguageProficiencyIds => _languageProficiencyIds.AsReadOnly();
    public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

    private Class()
    {
    }

    private Class(ClassId id, Name name) : base(id)
    {
        Name = name;
    }

    public static Class Create(Name name)
        => new Class(ClassId.New(), name);

    public void AddLanguageProficiency(LanguageProficiencyId languageProficiencyId)
    {
        if (_languageProficiencyIds.Contains(languageProficiencyId))
        {
            throw new DomainException("Language proficiency already exists");
        }

        _languageProficiencyIds.Add(languageProficiencyId);
    }

    public void RemoveLanguageProficiency(LanguageProficiencyId languageProficiencyId)
    {
        if (!_languageProficiencyIds.Contains(languageProficiencyId))
        {
            throw new DomainException("Language proficiency not found");
        }

        _languageProficiencyIds.Remove(languageProficiencyId);
    }

    public void AddLesson(Lesson lesson)
    {
        if (_lessons.Exists(x => x.Day == lesson.Day && x.StartTime <= lesson.EndTime && x.EndTime >= lesson.StartTime))
            throw new DomainException("Lesson is in class hours");

        _lessons.Add(lesson);
    }
}