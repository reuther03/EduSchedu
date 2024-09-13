﻿using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public record AddClassLessonCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid ClassId,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime
) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddClassLessonCommand, Guid>
    {
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUserService _userService;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUnitOfWork _unitOfWork;

        public Handler(
            ISchoolUserRepository schoolUserRepository,
            IUserService userService,
            ISchoolRepository schoolRepository,
            ISchoolUnitOfWork unitOfWork)
        {
            _schoolUserRepository = schoolUserRepository;
            _userService = userService;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddClassLessonCommand request, CancellationToken cancellationToken)
        {
            var admin = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            if (admin is null)
                return Result<Guid>.BadRequest("User not found");


            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            if (school is null)
                return Result<Guid>.BadRequest("School not found");

            if (admin.Role == Role.Teacher && !school.TeacherIds.Contains(admin.Id))
                return Result<Guid>.BadRequest("You are not allowed to create class");

            var @class = await _schoolRepository.GetClassByIdAsync(request.SchoolId, request.ClassId, cancellationToken);
            if (@class is null)
                return Result<Guid>.BadRequest("Class not found");

            var lesson = Lesson.Create(request.DayOfWeek, request.StartTime, request.EndTime);

            @class.AddLesson(lesson);
            await _schoolRepository.AddLessonAsync(lesson, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(lesson.Id);
        }
    }
}