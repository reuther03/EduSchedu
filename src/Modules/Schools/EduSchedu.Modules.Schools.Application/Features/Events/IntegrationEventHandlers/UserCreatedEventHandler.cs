﻿using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Events.IntegrationEventHandlers;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ISchoolUserRepository _schoolUserRepository;
    private readonly ISchoolRepository _schoolRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public UserCreatedEventHandler(
        ISchoolUserRepository schoolUserRepository,
        ISchoolRepository schoolRepository,
        IUserService userService,
        IUnitOfWork unitOfWork, IPublisher publisher)
    {
        _schoolUserRepository = schoolUserRepository;
        _schoolRepository = schoolRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var headmaster = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
        if (headmaster is null)
            return;

        var school = await _schoolRepository.GetByIdAsync(notification.SchoolId, cancellationToken);
        if (school is null)
            return;

        if (school.HeadmasterId != headmaster.Id)
            return;

        if (await _schoolUserRepository.ExistsAsync(new UserId(notification.UserId), cancellationToken))
            return;

        SchoolUser user = null!;
        switch (notification.Role)
        {
            case Role.Teacher:
            {
                var teacher = Teacher.Create(new UserId(notification.UserId), new Email(notification.Email), new Name(notification.FullName));
                // schedule = Schedule.Create(teacher.Id);
                // teacher.SetSchedule(schedule);
                school.AddTeacher(teacher.Id);
                user = teacher;
                break;
            }

            case Role.HeadMaster:
                break;

            case Role.Student:
                var student = Student.Create(new UserId(notification.UserId), new Email(notification.Email), new Name(notification.FullName));
                // schedule = Schedule.Create(student.Id);
                // student.SetSchedule(schedule);
                school.AddStudent(student.Id);
                user = student;
                break;

            case Role.BackOffice:
                // plan: chyba nie powinien sie tworzyc backoffice user
                // user = BackOfficeUser.Create(new UserId(notification.UserId), new Email(notification.Email), new Name(notification.FullName));
                // school.AddTeacher(user.Id);
                break;

            default:
                throw new ArgumentException("Invalid role.");
        }

        await _schoolUserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        await _publisher.Publish(new SchoolUserCreatedEvent(user.Id, user.Role), cancellationToken);
    }
}