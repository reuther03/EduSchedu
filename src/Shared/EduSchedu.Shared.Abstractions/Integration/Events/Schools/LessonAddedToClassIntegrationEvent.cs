using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Schools;

public record LessonAddedToClassIntegrationEvent(List<UserId> StudentIds, LessonPayload LessonPayload) : INotification;