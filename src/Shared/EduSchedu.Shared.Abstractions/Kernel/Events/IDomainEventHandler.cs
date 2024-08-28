using MediatR;

namespace EduSchedu.Shared.Abstractions.Kernel.Events;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;