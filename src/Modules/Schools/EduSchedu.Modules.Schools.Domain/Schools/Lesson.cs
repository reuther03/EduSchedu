using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schools.Domain.Schools;

public class Lesson : Entity<Guid>
{
    public DayOfWeek Day { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    private Lesson()
    {
    }

    private Lesson(Guid id, DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
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

        var lesson = new Lesson(Guid.NewGuid(), day, startTime, endTime);
        return lesson;
    }

}