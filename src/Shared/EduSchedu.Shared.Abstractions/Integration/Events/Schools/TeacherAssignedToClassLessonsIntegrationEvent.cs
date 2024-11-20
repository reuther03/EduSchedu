using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Schools;

public record TeacherAssignedToClassLessonsIntegrationEvent(Dictionary<UserId, List<ScheduleItemPayload>> TeacherLessons) : INotification;