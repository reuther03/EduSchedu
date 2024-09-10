using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Lesson : AggregateRoot<LessonId>
{
    private readonly List<UserId> _teacherIds = [];
    private readonly List<ClassId> _classIds = [];

    public DayOfWeek Day { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public IReadOnlyList<UserId> TeacherIds => _teacherIds.AsReadOnly();
    public IReadOnlyList<ClassId> ClassIds => _classIds.AsReadOnly();

    private Lesson()
    {
    }

    private Lesson(LessonId id, DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
        : base(id)
    {
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Lesson Create(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
        {
            throw new DomainException("Start time must be before end time");
        }

        var lesson = new Lesson(LessonId.New(), day, startTime, endTime);
        return lesson;
    }

    public void AddTeacher(UserId teacherId)
    {
        if (_teacherIds.Contains(teacherId))
        {
            throw new DomainException("Teacher already exists");
        }

        _teacherIds.Add(teacherId);
    }

    public void AddClass(ClassId classId)
    {
        if (_classIds.Contains(classId))
        {
            throw new DomainException("Class already exists");
        }

        _classIds.Add(classId);
    }
}