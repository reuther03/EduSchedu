using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Users;

public record HeadmasterCreatedEvent(Guid UserId, string FullName, string Email) : INotification;