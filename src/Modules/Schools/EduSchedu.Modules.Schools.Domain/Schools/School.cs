using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class School : AggregateRoot<SchoolId>
{
    private readonly List<Teacher> _teachers = [];
    private readonly List<ClassId> _classIds = [];
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public Email Email { get; private set; }
    public UserId PrincipalId { get; private set; }

    public IReadOnlyList<Teacher> Teachers => _teachers.AsReadOnly();
    public IReadOnlyList<ClassId> ClassIds => _classIds.AsReadOnly();

    private School()
    {
    }

    public School(SchoolId id, Name name, Address address, string phoneNumber, Email email, UserId principalId) : base(id)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        PrincipalId = principalId;
    }

    public static School Create(Name name, Address address, string phoneNumber, Email email, UserId principalId)
        => new School(SchoolId.New(), name, address, phoneNumber, email, principalId);

    public void AddTeacher(Teacher teacher)
    {
        if (_teachers.Exists(x => x.Id == teacher.Id))
            throw new DomainException("Teacher already exists");

        _teachers.Add(teacher);
    }

    public void AddClass(ClassId classId)
    {
        if (_classIds.Contains(classId))
            throw new DomainException("Class already exists");

        _classIds.Add(classId);
    }
}