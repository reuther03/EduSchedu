using EduSchedu.Shared.Abstractions.Kernel.Events;

namespace EduSchedu.Shared.Abstractions.Events.DomainEvents;

public record UserAssignedDomainEvent(Guid UserId, Guid SchoolId, Guid ClassId) : IDomainEvent;