using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Events.IntegrationEventHandlers;

public class BackOfficeUserCreatedEventHandler : INotificationHandler<BackOfficeUserCreatedEvent>
{
    private readonly ISchoolRepository _schoolRepository;
    private readonly ISchoolUserRepository _schoolUserRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BackOfficeUserCreatedEventHandler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUnitOfWork unitOfWork)
    {
        _schoolRepository = schoolRepository;
        _schoolUserRepository = schoolUserRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(BackOfficeUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var schools = await _schoolRepository.GetAllAsync(cancellationToken);
        var backOfficeUser = BackOfficeUser.Create(notification.UserId, notification.Email, notification.FullName);
        foreach (var school in schools)
        {
            school.AddTeacher(backOfficeUser.Id);
        }

        await _schoolUserRepository.AddAsync(backOfficeUser, cancellationToken);
    }
}