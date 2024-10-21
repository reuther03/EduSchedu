using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Events;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Events;

public class HeadMasterCreatedEventHandler : INotificationHandler<HeadmasterCreatedEvent>
{
    private readonly ISchoolUserRepository _schoolUserRepository;
    private readonly ISchoolUnitOfWork _schoolUnitOfWork;

    public HeadMasterCreatedEventHandler(ISchoolUserRepository schoolUserRepository, ISchoolUnitOfWork schoolUnitOfWork)
    {
        _schoolUserRepository = schoolUserRepository;
        _schoolUnitOfWork = schoolUnitOfWork;
    }

    public async Task Handle(HeadmasterCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (await _schoolUserRepository.ExistsAsync(new UserId(notification.UserId), cancellationToken))
            return;

        var user = Teacher.Create(new UserId(notification.UserId), new Email(notification.Email), new Name(notification.FullName), Role.HeadMaster);
        var schedule = Schedule.Create(user.Id);
        user.SetSchedule(schedule);

        await _schoolUserRepository.AddAsync(user, cancellationToken);
        await _schoolUnitOfWork.CommitAsync(cancellationToken);
    }
}