using MediatR;

namespace EduSchedu.Shared.Abstractions.Events;

public record HeadmasterCreatedEvent(Guid UserId, string FullName, string Email) : INotification;