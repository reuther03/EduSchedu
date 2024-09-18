using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Lesson : Entity<Guid>
{
    public DayOfWeek Day { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public UserId? AssignedTeacher { get; private set; }
    public ScheduleId? ScheduleId { get; private set; }

    private Lesson()
    {
    }

    private Lesson(Guid id, DayOfWeek day, TimeOnly startTime, TimeOnly endTime) : base(id)
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

        var lesson = new Lesson(Guid.NewGuid(), day, startTime, endTime);
        return lesson;
    }

    public void AssignTeacher(UserId teacherId)
        => AssignedTeacher = teacherId;
}