using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Integration.Events.EventPayloads;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;
using MediatR;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public record AddStudentToClassCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid ClassId,
    Guid StudentId) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddStudentToClassCommand, Guid>
    {
        private readonly IUserService _userService;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisher _publisher;

        public Handler(IUserService userService, ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUnitOfWork unitOfWork,
            IPublisher publisher)
        {
            _userService = userService;
            _schoolRepository = schoolRepository;
            _schoolUserRepository = schoolUserRepository;
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }

        public async Task<Result<Guid>> Handle(AddStudentToClassCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _schoolUserRepository.GetTeacherByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(teacher);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            var student = await _schoolUserRepository.GetStudentByIdAsync(request.StudentId, cancellationToken);
            NullValidator.ValidateNotNull(student);

            if (!school.StudentIds.Contains(student.Id))
                return Result<Guid>.BadRequest("Student is not a student of this school");

            var @class = school.Classes.FirstOrDefault(x => x.Id == Domain.Schools.Ids.ClassId.From(request.ClassId));
            NullValidator.ValidateNotNull(@class);

            if (@class.StudentIds.Contains(student.Id))
                return Result<Guid>.BadRequest("Student is already in this class");

            if (@class.Lessons.All(x => x.AssignedTeacher != teacher.Id) && teacher.Id != school.HeadmasterId)
                return Result<Guid>.BadRequest("You are not allowed to add student to this class");

            @class.AddStudent(student.Id);
            await _unitOfWork.CommitAsync(cancellationToken);
            await _publisher.Publish(new StudentAddedToClassEvent(student.Id, @class.Lessons
                .Select(x => new LessonPayload
                    {
                        Day = x.Day,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime
                    }
                ).ToList()), cancellationToken);

            return Result<Guid>.Ok(@class.Id.Value);
        }
    }
}