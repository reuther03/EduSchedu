﻿using MediatR;

namespace EduSchedu.Shared.Abstractions.Events;

public record UserCreatedEvent(Guid UserId, string FullName, string Email) : INotification;