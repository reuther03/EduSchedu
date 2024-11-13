using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Users;

public record SchoolUserCreatedEvent(Guid UserId, Role Role) : INotification;