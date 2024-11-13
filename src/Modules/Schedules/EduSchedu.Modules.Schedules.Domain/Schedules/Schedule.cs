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

    public void AddItem(ScheduleItem item)
    {
        _scheduleItems.Add(item);
    }
}