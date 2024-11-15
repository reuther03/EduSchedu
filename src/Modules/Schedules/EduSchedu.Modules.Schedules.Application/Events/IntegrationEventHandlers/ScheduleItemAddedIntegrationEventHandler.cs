using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using MediatR;

namespace EduSchedu.Modules.Schedules.Application.Events.IntegrationEventHandlers;

public class ScheduleItemAddedIntegrationEventHandler : INotificationHandler<ScheduleItemAddedIntegrationEvent>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleItemAddedIntegrationEventHandler(IScheduleRepository scheduleRepository, IUnitOfWork unitOfWork)
    {
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ScheduleItemAddedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var schedule = await _scheduleRepository.GetScheduleByUserIdAsync(notification.UserId, cancellationToken);
        NullValidator.ValidateNotNull(schedule);

        var scheduleItem = ScheduleItem.Create(notification.Type, notification.Day, notification.Start, notification.End);
        schedule.AddItem(scheduleItem);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}