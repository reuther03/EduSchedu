using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schedules.Domain.Schedules;

public class Schedule : AggregateRoot<ScheduleId>
{
    private readonly List<ScheduleItem> _items = [];

    public UserId UserId { get; private set; }
    public IReadOnlyList<ScheduleItem> Items => _items.AsReadOnly();

    private Schedule()
    {
    }

    private Schedule(ScheduleId id, UserId userId) : base(id)
    {
        UserId = userId;
    }

    public void AddItem(ScheduleItem item)
    {
        _items.Add(item);
    }
}