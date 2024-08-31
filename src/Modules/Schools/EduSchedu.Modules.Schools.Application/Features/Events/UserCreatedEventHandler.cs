using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Events;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Events;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserCreatedEventHandler(ITeacherRepository teacherRepository, IUnitOfWork unitOfWork)
    {
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        if (await _teacherRepository.ExistsAsync(new UserId(notification.UserId), cancellationToken))
            return;

        var user = Teacher.Create(new UserId(notification.UserId), new Email(notification.Email), new Name(notification.FullName), Role.Teacher);

        await _teacherRepository.AddAsync(user, cancellationToken);
        await _teacherRepository.SaveChangesAsync(cancellationToken);
    }
}