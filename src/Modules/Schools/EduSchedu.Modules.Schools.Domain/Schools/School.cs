using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class School : AggregateRoot<SchoolId>
{
    //todo: cos w stylu wag ocen: bool prop czy szkola ma wagi ocen czy nie/ liczyc srednia z wagami: waga * ocena/ czy szkola lub kalsa ma lekcje online
    private readonly List<UserId> _teacherIds = [];
    private readonly List<UserId> _studentIds = [];
    private readonly List<Class> _classes = [];
    private readonly List<SchoolApplication> _schoolApplications = [];
    // private readonly List<ApplicationConsideration> _applicationConsiderations = [];
    public Name Name { get; private set; }
    public Address Address { get; private set; }
    public string PhoneNumber { get; private set; }
    public Email Email { get; private set; }
    public UserId HeadmasterId { get; private set; }

    public IReadOnlyList<UserId> TeacherIds => _teacherIds.AsReadOnly();
    public IReadOnlyList<UserId> StudentIds => _studentIds.AsReadOnly();
    public IReadOnlyList<Class> Classes => _classes.AsReadOnly();
    public IReadOnlyList<SchoolApplication> SchoolApplications => _schoolApplications.AsReadOnly();
    // public IReadOnlyList<ApplicationConsideration> ApplicationConsiderations => _applicationConsiderations.AsReadOnly();

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
        => new(SchoolId.New(), name, address, phoneNumber, email, principalId);

    public void AddTeacher(UserId teacherId)
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

    public void AddStudent(UserId studentId)
    {
        if (_studentIds.Contains(studentId))
            throw new DomainException("Student already exists");

        _studentIds.Add(studentId);
    }

    public void AddApplication(SchoolApplication application)
    {
        //todo: to powinno jeszcze sprawdzac consideration status i jesli jest canceled albo rejected to powinno pozwolic dodac
        //plan: domain event ktory bedzie dodawal consideration application do listy consideration applications z consideration status pending
        if (_schoolApplications.Exists(x => x.UserId == application.UserId))
            throw new DomainException("Application already exists");

        _schoolApplications.Add(application);
    }
}