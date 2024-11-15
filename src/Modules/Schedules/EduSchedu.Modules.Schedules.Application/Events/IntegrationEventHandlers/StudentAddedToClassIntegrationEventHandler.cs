using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;

namespace EduSchedu.Modules.Schedules.Application.Events.IntegrationEventHandlers;

public class StudentAddedToClassIntegrationEventHandler : INotificationHandler<StudentAddedToClassEvent>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;

    public StudentAddedToClassIntegrationEventHandler(IScheduleRepository scheduleRepository, IUserService userService, IUnitOfWork unitOfWork)
    {
        _scheduleRepository = scheduleRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(StudentAddedToClassEvent notification, CancellationToken cancellationToken)
    {
        var schedule = await _scheduleRepository.GetScheduleByUserIdAsync(notification.UserId, cancellationToken);
        NullValidator.ValidateNotNull(schedule);

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var lesson in notification.Lessons)
        {
            var scheduleItem = ScheduleItem.Create(ScheduleItemType.Lesson, lesson.Day, lesson.StartTime, lesson.EndTime);
            schedule.AddItem(scheduleItem);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}