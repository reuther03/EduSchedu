using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users.Students;

public class Student : SchoolUser
{
    private readonly List<Grade> _grades = [];
    public IReadOnlyList<Grade> Grades => _grades;

    public double AverageGrade
    {
        get => _grades.Count != 0 ? _grades.Average(x => x.GradeValue) : 0;
        private set { }
    }

    private Student()
    {
    }

    public Student(UserId id, Email email, Name fullName, Role role) : base(id, email, fullName, role)
    {
    }

    public static Student Create(UserId id, Email email, Name fullName)
    {
        return new Student(id, email, fullName, Role.Student);
    }

    public void AddGrade(Grade grade)
    {
        _grades.Add(grade);
    }
}