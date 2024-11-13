using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Users;

public record SchoolUserCreatedEvent(Guid UserId) : INotification;