using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Class : Entity<ClassId>
{
    private readonly List<LanguageProficiencyId> _languageProficiencyIds = [];

    public Name Name { get; private set; }
    public IReadOnlyList<LanguageProficiencyId> LanguageProficiencyIds => _languageProficiencyIds.AsReadOnly();

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
}