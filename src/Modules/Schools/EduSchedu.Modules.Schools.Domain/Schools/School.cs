using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class School : AggregateRoot<SchoolId>
{
    private readonly List<Student> _students = [];
    private readonly List<Teacher> _teachers = [];
    private readonly List<ClassId> _classes = [];

    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public Email Email { get; private set; }
    public UserId PrincipalId { get; private set; }

    public IReadOnlyList<Student> Students => _students.AsReadOnly();
    public IReadOnlyList<Teacher> Teachers => _teachers.AsReadOnly();
    public IReadOnlyList<ClassId> Classes => _classes.AsReadOnly();

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

    public void AddStudent(Student student)
    {
        if (_students.Any(x => x.Id == student.Id))
            throw new DomainException("Student already exists");

        _students.Add(student);
    }

    public void AddTeacher(Teacher teacher)
    {
        if (_teachers.Any(x => x.Id == teacher.Id))
            throw new DomainException("Teacher already exists");

        _teachers.Add(teacher);
    }

    public void AddClass(ClassId classId)
    {
        if (_classes.Any(x => x == classId))
            throw new DomainException("Class already exists");

        _classes.Add(classId);
    }
}