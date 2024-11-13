using EduSchedu.Modules.Schools.Domain.Schools.Enums;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class LanguageProficiency : Entity<Guid>
{
    public Language Language { get; private set; }
    public Lvl Lvl { get; private set; }

    private LanguageProficiency()
    {
    }

    private LanguageProficiency(Guid id, Language language, Lvl lvl)
        : base(id)
    {
        Language = language;
        Lvl = lvl;
    }

    public static LanguageProficiency Create(Guid id, Language language, Lvl lvl)
        => new(id, language, lvl);
}