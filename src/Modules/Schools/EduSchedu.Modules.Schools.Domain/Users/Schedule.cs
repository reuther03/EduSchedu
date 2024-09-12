using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Schedule : AggregateRoot<ScheduleId>
{
    private readonly List<Lesson> _lessons = [];
    private readonly List<ScheduleItem> _scheduleItems = [];


    public IReadOnlyList<ScheduleItem> ScheduleItems => _scheduleItems;
    public UserId TeacherId { get; private set; }
    public Teacher Teacher { get; private set; }

    public IReadOnlyList<Lesson> Lessons => _lessons;

    private Schedule()
    {
    }

    private Schedule(ScheduleId id, UserId teacherId) : base(id)
    {
        TeacherId = teacherId;
    }

    public static Schedule Create(ScheduleId id, UserId teacherId)
        => new Schedule(id, teacherId);

    public void AddLesson(Lesson lesson)
    {
        if (_lessons.Exists(x => x.Day == lesson.Day && x.StartTime <= lesson.EndTime && x.EndTime >= lesson.StartTime))
            throw new DomainException("Lesson is in class hours");

        _lessons.Add(lesson);
    }

    public void AddScheduleItem(ScheduleItem item)
        => _scheduleItems.Add(item);
}