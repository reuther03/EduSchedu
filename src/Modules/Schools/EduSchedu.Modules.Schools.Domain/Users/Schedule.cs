using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Schedule : AggregateRoot<ScheduleId>
{
    private readonly List<ScheduleItem> _scheduleItems = [];

    public UserId SchoolUserId { get; private set; }
    public SchoolUser SchoolUser { get; private set; }

    public IReadOnlyList<ScheduleItem> ScheduleItems => _scheduleItems;

    private Schedule()
    {
    }

    private Schedule(ScheduleId id, UserId schoolUserId) : base(id)
    {
        SchoolUserId = schoolUserId;
    }

    public static Schedule Create(UserId teacherId)
        => new(ScheduleId.New(), teacherId);

    public void AddScheduleItem(ScheduleItem item)
        => _scheduleItems.Add(item);

    public void RemoveScheduleItem(ScheduleItem item)
    {
        if (!_scheduleItems.Contains(item))
            throw new DomainException("Schedule item does not exist");

        _scheduleItems.Remove(item);
    }
}