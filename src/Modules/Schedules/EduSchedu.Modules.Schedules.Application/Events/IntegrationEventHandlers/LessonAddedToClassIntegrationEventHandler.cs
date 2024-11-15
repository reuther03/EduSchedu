using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Integration.Schools;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;

namespace EduSchedu.Modules.Schedules.Application.Events.IntegrationEventHandlers;

public class LessonAddedToClassIntegrationEventHandler : INotificationHandler<LessonAddedToClassIntegrationEvent>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;

    public LessonAddedToClassIntegrationEventHandler(IScheduleRepository scheduleRepository, IUserService userService, IUnitOfWork unitOfWork)
    {
        _scheduleRepository = scheduleRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(LessonAddedToClassIntegrationEvent notification, CancellationToken cancellationToken)
    {
        if (_userService.Role != Role.HeadMaster && _userService.Role != Role.Teacher)
            throw new UnauthorizedAccessException("Only headmaster and teacher can add lesson to class");

        var userSchedules = await _scheduleRepository.GetSchedulesByUserIdsAsync(notification.StudentIds, cancellationToken);
        NullValidator.ValidateNotNull(userSchedules);

        foreach (var schedule in userSchedules)
        {
            var scheduleItem = ScheduleItem.Create
            (
                ScheduleItemType.Lesson,
                notification.LessonPayload.Day,
                notification.LessonPayload.StartTime,
                notification.LessonPayload.EndTime
            );
            schedule.AddItem(scheduleItem);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}