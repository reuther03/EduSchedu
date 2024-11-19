using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Schools;

//og usunac to czy cos
public record AssignTeacherToClassLessonsIntegrationEvent(List<UserId> UserIds, List<ScheduleItemPayload> Lessons) : INotification;