using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Schedule : AggregateRoot<ScheduleId>
{
    // private readonly List<Lesson> _lessons = [];
    private readonly List<ScheduleItem> _scheduleItems = [];


    public UserId TeacherId { get; private set; }
    public Teacher Teacher { get; private set; }

    // public IReadOnlyList<Lesson> Lessons => _lessons;
    public IReadOnlyList<ScheduleItem> ScheduleItems => _scheduleItems;

    private Schedule()
    {
    }

    private Schedule(ScheduleId id, UserId teacherId) : base(id)
    {
        TeacherId = teacherId;
    }

    public static Schedule Create(UserId teacherId)
        => new (ScheduleId.New(), teacherId);

    public void AddScheduleItem(ScheduleItem item)
        => _scheduleItems.Add(item);

    public void RemoveScheduleItem(ScheduleItem item)
    {
        if (!_scheduleItems.Contains(item))
            throw new DomainException("Schedule item does not exist");

        _scheduleItems.Remove(item);
    }
}