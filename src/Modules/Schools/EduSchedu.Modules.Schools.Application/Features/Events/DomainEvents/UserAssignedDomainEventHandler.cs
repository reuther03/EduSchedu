using EduSchedu.Modules.Schools.Application.Abstractions.Database;
using EduSchedu.Shared.Abstractions.Events.DomainEvents;
using EduSchedu.Shared.Abstractions.Kernel.Events;

namespace EduSchedu.Modules.Schools.Application.Features.Events.DomainEvents;

public class UserAssignedDomainEventHandler : IDomainEventHandler<UserAssignedDomainEvent>
{
    private readonly ISchoolsDbContext SchoolsDbContext;

    public UserAssignedDomainEventHandler(ISchoolsDbContext schoolsDbContext)
    {
        SchoolsDbContext = schoolsDbContext;
    }

    public Task Handle(UserAssignedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}