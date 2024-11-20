using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Integration.Events.Schools;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Modules.Schedules.Application.Events.IntegrationEventHandlers;

public class TeacherAssignedToClassLessonsIntegrationEventHandler : INotificationHandler<TeacherAssignedToClassLessonsIntegrationEvent>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TeacherAssignedToClassLessonsIntegrationEventHandler(IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork)
    {
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TeacherAssignedToClassLessonsIntegrationEvent notification, CancellationToken cancellationToken)
    {
        foreach (var key in notification.TeacherLessons.Keys)
        {
            var schedule = await _scheduleRepository.GetScheduleByUserIdAsync(key, cancellationToken);
            NullValidator.ValidateNotNull(schedule);

            foreach (var lesson in notification.TeacherLessons[key])
            {
                schedule.AddItem(ScheduleItem.Create(ScheduleItemType.Lesson, lesson.Day, lesson.StartTime, lesson.EndTime));
            }
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}