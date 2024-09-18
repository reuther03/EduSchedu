using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Class : Entity<ClassId>
{
    private readonly List<Lesson> _lessons = [];

    public Name Name { get; private set; }
    public LanguageProficiency? LanguageProficiency { get; private set; }
    public IReadOnlyList<Lesson> Lessons => _lessons.AsReadOnly();

    private Class()
    {
    }

    private Class(ClassId id, Name name) : base(id)
    {
        Name = name;
    }

    public static Class Create(Name name)
        => new(ClassId.New(), name);

    public void SetLanguageProficiency(LanguageProficiency languageProficiency)
    {
        if (LanguageProficiency == languageProficiency)
            throw new DomainException("Language proficiency is already set");

        LanguageProficiency = languageProficiency;
    }

    public void AddLesson(Lesson lesson)
    {
        if (_lessons.Exists(x => x.Day == lesson.Day && x.StartTime <= lesson.EndTime && x.EndTime >= lesson.StartTime))
            throw new DomainException("Lesson is in class hours");

        _lessons.Add(lesson);
    }
}