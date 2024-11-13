using System.ComponentModel.DataAnnotations;
using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
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
        if (_userService.Role != Role.HeadMaster && _userService.Role != Role.Teacher)
            throw new UnauthorizedAccessException("Only headmaster and teacher can add student to class");

        var schedule = await _scheduleRepository.GetScheduleByUserIdAsync(notification.UserId, cancellationToken);
        if (schedule is null)
            throw new ValidationException("User does not have a schedule");

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var lesson in notification.Lessons)
        {
            var scheduleItem = ScheduleItem.Create(ScheduleItemType.Lesson, lesson.Day, lesson.StartTime, lesson.EndTime);
            schedule.AddItem(scheduleItem);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}