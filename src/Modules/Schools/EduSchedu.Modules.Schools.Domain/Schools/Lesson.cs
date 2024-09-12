using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Lesson : AggregateRoot<LessonId>
{
    private readonly List<ClassId> _classIds = [];

    public DayOfWeek Day { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public UserId? AssignedTeacher { get; private set; }

    public IReadOnlyList<ClassId> ClassIds => _classIds.AsReadOnly();

    private Lesson()
    {
    }

    private Lesson(LessonId id, DayOfWeek day, TimeOnly startTime, TimeOnly endTime) : base(id)
    {
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Lesson Create(DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
        {
            throw new DomainException("Start time must be before end time");
        }

        var lesson = new Lesson(LessonId.New(), day, startTime, endTime);
        return lesson;
    }


    public void AddClass(ClassId classId)
    {
        if (_classIds.Contains(classId))
        {
            throw new DomainException("Class already exists");
        }

        _classIds.Add(classId);
    }

    public void AssignTeacher(UserId teacherId)
    {
        AssignedTeacher = teacherId;
    }
}