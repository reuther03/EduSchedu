using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schedules.Domain.Schedules;

public class Schedule : AggregateRoot<ScheduleId>
{
    private readonly List<ScheduleItem> _scheduleItems = [];

    public UserId UserId { get; private set; }
    public IReadOnlyList<ScheduleItem> ScheduleItems => _scheduleItems.AsReadOnly();

    private Schedule()
    {
    }

    private Schedule(ScheduleId id, UserId userId) : base(id)
    {
        UserId = userId;
    }

    public static Schedule Create(UserId userId)
    {
        var schedule = new Schedule(ScheduleId.New(), userId);
        return schedule;
    }

    public void AddItem(ScheduleItem item)
    {
        if (_scheduleItems.Exists(x => x.Day == item.Day && x.StartTime == item.StartTime && x.EndTime == item.EndTime))
            throw new DomainException("There is already a activity at this time");

        _scheduleItems.Add(item);
    }
}