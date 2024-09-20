using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Events;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Events;

public class BackOfficeUserCreatedEventHandler : INotificationHandler<BackOfficeUserCreatedEvent>
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly ISchoolUserRepository _schoolUserRepository;
    private readonly ISchoolUnitOfWork _schoolUnitOfWork;

    public BackOfficeUserCreatedEventHandler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, ISchoolUnitOfWork schoolUnitOfWork)
    {
        _schoolRepository = schoolRepository;
        _schoolUserRepository = schoolUserRepository;
        _schoolUnitOfWork = schoolUnitOfWork;
    }

    public async Task Handle(BackOfficeUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var schools = await _schoolRepository.GetAllAsync(cancellationToken);
        var backOfficeUser = BackOfficeUser.Create(notification.UserId, notification.Email, notification.FullName);
        foreach (var school in schools)
        {
            school.AddUser(backOfficeUser.Id);
        }

        await _schoolUserRepository.AddAsync(backOfficeUser, cancellationToken);
    }
}