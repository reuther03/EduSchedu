﻿using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Teacher : SchoolUser
{
    private readonly List<LanguageProficiencyId> _languageProficiencyIds = [];
    public IReadOnlyList<LanguageProficiencyId> LanguageProficiencyIds => _languageProficiencyIds.AsReadOnly();

    private Teacher()
    {
    }

    protected Teacher(UserId id, Email email, Name fullName, Role role)
        : base(id, email, fullName, role)
    {
    }

    public static Teacher Create(UserId id, Email email, Name fullName)
    {
        var teacher = new Teacher(id, email, fullName, Role.Teacher);
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
}