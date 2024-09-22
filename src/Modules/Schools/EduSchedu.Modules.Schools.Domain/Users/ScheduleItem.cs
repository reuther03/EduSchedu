using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class ScheduleItem : Entity<Guid>
{
    public ScheduleItemType Type { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeOnly Start { get; private set; }
    public TimeOnly End { get; private set; }
    public Description Description { get; private set; }

    private ScheduleItem()
    {
    }

    private ScheduleItem(Guid id, ScheduleItemType type, DayOfWeek day, TimeOnly start, TimeOnly end, Description description) : base(id)
    {
        Type = type;
        Day = day;
        Start = start;
        End = end;
        Description = description;
    }

    public static ScheduleItem Create(ScheduleItemType type, DayOfWeek day, TimeOnly start, TimeOnly end, string description)
    {
        if (start >= end)
        {
            throw new DomainException("Start time must be before end time");
        }

        var scheduleItem = new ScheduleItem(Guid.NewGuid(), type, day, start, end, description);
        return scheduleItem;
    }

    public static ScheduleItem Create(ScheduleItemType type, DayOfWeek day, TimeOnly start, TimeOnly end)
    {
        if (start >= end)
        {
            throw new DomainException("Start time must be before end time");
        }

        var scheduleItem = new ScheduleItem(Guid.NewGuid(), type, day, start, end, "Lesson");
        return scheduleItem;
    }
}