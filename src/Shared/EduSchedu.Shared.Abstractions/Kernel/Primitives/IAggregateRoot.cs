using EduSchedu.Shared.Abstractions.Kernel.Events;

namespace EduSchedu.Shared.Abstractions.Kernel.Primitives;

public interface IAggregateRoot
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void RaiseDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();
}