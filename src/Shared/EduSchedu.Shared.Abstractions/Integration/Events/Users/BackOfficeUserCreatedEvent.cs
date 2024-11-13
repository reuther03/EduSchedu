using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Users;

public record BackOfficeUserCreatedEvent(Guid UserId, string FullName, string Email, Role Role) : INotification;