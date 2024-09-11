using EduSchedu.Modules.Schools.Domain.Schools.Ids;
using EduSchedu.Shared.Abstractions.Exception;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;

namespace EduSchedu.Modules.Schools.Domain.Users;

public class Schedule : AggregateRoot<ScheduleId>
{
    private readonly List<LessonId> _lessonIds = [];
    private readonly List<ScheduleItem> _scheduleItems = [];


    public IReadOnlyList<ScheduleItem> ScheduleItems => _scheduleItems;
    public UserId TeacherId { get; private set; }
    public Teacher Teacher { get; private set; }

    public IReadOnlyList<LessonId> LessonIds => _lessonIds.AsReadOnly();

    private Schedule()
    {
    }

    private Schedule(ScheduleId id, UserId teacherId) : base(id)
    {
        TeacherId = teacherId;
    }

    public static Schedule Create(ScheduleId id, UserId teacherId)
        => new Schedule(id, teacherId);

    public void AddLesson(LessonId lessonId)
    {
        if (_lessonIds.Contains(lessonId))
        {
            throw new DomainException("Lesson already exists");
        }

        _lessonIds.Add(lessonId);
    }

    public void AddScheduleItem(ScheduleItem item)
        => _scheduleItems.Add(item);
}