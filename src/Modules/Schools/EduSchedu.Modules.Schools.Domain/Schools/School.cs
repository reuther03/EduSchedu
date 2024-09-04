using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class School : AggregateRoot<SchoolId>
{
    private readonly List<UserId> _teacherIds = [];
    private readonly List<Class> _classes = [];
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public Email Email { get; private set; }
    public UserId HeadmasterId { get; private set; }

    public IReadOnlyList<UserId> TeacherIds => _teacherIds.AsReadOnly();
    public IReadOnlyList<Class> Classes => _classes.AsReadOnly();

    private School()
    {
    }

    public School(SchoolId id, Name name, Address address, string phoneNumber, Email email, UserId headmasterId) : base(id)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        HeadmasterId = headmasterId;
    }

    public static School Create(Name name, Address address, string phoneNumber, Email email, UserId principalId)
        => new School(SchoolId.New(), name, address, phoneNumber, email, principalId);

    public void AddUser(UserId teacherId)
    {
        if (_teacherIds.Contains(teacherId))
            throw new DomainException("Teacher already exists");

        _teacherIds.Add(teacherId);
    }

    public void AddClass(Class @class)
    {
        if (_classes.Exists(x => x.Name == @class.Name))
            throw new DomainException("Class already exists");

        _classes.Add(@class);
    }
}