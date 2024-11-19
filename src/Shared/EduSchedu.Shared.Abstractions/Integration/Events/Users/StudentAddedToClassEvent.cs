using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Users;

public record StudentAddedToClassEvent(Guid UserId, List<ScheduleItemPayload> Lessons) : INotification;