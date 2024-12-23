﻿using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Events.IntegrationEventHandlers;

public class HeadMasterCreatedEventHandler : INotificationHandler<HeadmasterCreatedEvent>
{
    private readonly ISchoolUserRepository _schoolUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public HeadMasterCreatedEventHandler(ISchoolUserRepository schoolUserRepository, IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _schoolUserRepository = schoolUserRepository;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task Handle(HeadmasterCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (await _schoolUserRepository.ExistsAsync(new UserId(notification.UserId), cancellationToken))
            return;

        var user = Headmaster.Create(new UserId(notification.UserId), new Email(notification.Email), new Name(notification.FullName));

        await _schoolUserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        await _publisher.Publish(new SchoolUserCreatedEvent(user.Id, user.Role), cancellationToken);
    }
}