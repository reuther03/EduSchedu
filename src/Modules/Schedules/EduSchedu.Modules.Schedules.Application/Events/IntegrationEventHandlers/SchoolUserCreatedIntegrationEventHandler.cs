using EduSchedu.Modules.Schedules.Application.Abstractions;
using EduSchedu.Modules.Schedules.Application.Abstractions.Database;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using MediatR;

namespace EduSchedu.Modules.Schedules.Application.Events.IntegrationEventHandlers;

public class SchoolUserCreatedIntegrationEventHandler : INotificationHandler<SchoolUserCreatedEvent>
{
    private readonly ISchedulesDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public SchoolUserCreatedIntegrationEventHandler(ISchedulesDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public Task Handle(SchoolUserCreatedEvent notification, CancellationToken cancellationToken)
    {

    }
}