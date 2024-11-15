using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Users;

public record ScheduleItemAddedIntegrationEvent(UserId UserId, ScheduleItemType Type, DayOfWeek Day, TimeOnly Start, TimeOnly End) : INotification;