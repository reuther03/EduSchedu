﻿using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Shared.Abstractions.Integration.Events.Schools;

public record LessonAddedToClassIntegrationEvent(List<UserId> StudentIds, ScheduleItemPayload ScheduleItemPayload) : INotification;