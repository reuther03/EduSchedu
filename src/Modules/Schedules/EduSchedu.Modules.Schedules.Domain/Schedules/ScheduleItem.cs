using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;

namespace EduSchedu.Modules.Schedules.Domain.Schedules;

public class ScheduleItem : Entity<Guid>
{
    public ScheduleItemType Type { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    private ScheduleItem()
    {
    }

    private ScheduleItem(Guid id, ScheduleItemType type, DayOfWeek day, TimeOnly startTime, TimeOnly endTime) : base(id)
    {
        Type = type;
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static ScheduleItem Create(ScheduleItemType type, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
            throw new DomainException("Start time must be before end time");

        var lesson = new ScheduleItem(Guid.NewGuid(), type, day, startTime, endTime);
        return lesson;
    }
}