using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Repositories;
using EduSchedu.Modules.Schedules.Domain.Schedules;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;

namespace EduSchedu.Modules.Schedules.Application.Events.IntegrationEventHandlers;

public class SchoolUserCreatedIntegrationEventHandler : INotificationHandler<SchoolUserCreatedEvent>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;

    public SchoolUserCreatedIntegrationEventHandler(IScheduleRepository scheduleRepository, IUserService userService, IUnitOfWork unitOfWork)
    {
        _scheduleRepository = scheduleRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SchoolUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        switch (notification.Role)
        {
            case Role.Student or Role.Teacher when _userService.Role is not Role.HeadMaster:
            {
                break;
            }
            case Role.HeadMaster or Role.Student or Role.Teacher:
            {
                var schedule = Schedule.Create(notification.UserId);

                await _scheduleRepository.AddAsync(schedule, cancellationToken);
                break;
            }
            default:
                return;
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}