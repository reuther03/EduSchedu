﻿using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Events;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Events;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ISchoolUserRepository _schoolUserRepository;
    private readonly ISchoolRepository _schoolRepository;
    private readonly IUserService _userService;
    private readonly ISchoolUnitOfWork _schoolUnitOfWork;

    public UserCreatedEventHandler(
        ISchoolUserRepository schoolUserRepository,
        ISchoolRepository schoolRepository,
        IUserService userService,
        ISchoolUnitOfWork schoolUnitOfWork
    )
    {
        _schoolUserRepository = schoolUserRepository;
        _schoolRepository = schoolRepository;
        _userService = userService;
        _schoolUnitOfWork = schoolUnitOfWork;
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
                var schedule = Schedule.Create(user.Id);
                teacher.SetSchedule(schedule);
                user = teacher;
                break;
            }

            case Role.HeadMaster:
                break;

            case Role.Student:
                var student = Student.Create(new UserId(notification.UserId), new Email(notification.Email), new Name(notification.FullName));
                // student.SetIndexNumber(school.Name);
                user = student;
                break;

            case Role.BackOffice:
                user = BackOfficeUser.Create(new UserId(notification.UserId), new Email(notification.Email), new Name(notification.FullName));
                break;

            default:
                throw new ArgumentException("Invalid role.");
        }

        school.AddUser(user.Id);
        await _schoolUserRepository.AddAsync(user, cancellationToken);
        await _schoolUnitOfWork.CommitAsync(cancellationToken);
    }
}