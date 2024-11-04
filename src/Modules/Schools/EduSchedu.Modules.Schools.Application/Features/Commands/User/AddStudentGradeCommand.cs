using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddStudentGradeCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid ClassId,
    [property: JsonIgnore]
    Guid StudentId,
    float Grade,
    int? Percentage,
    GradeType GradeType,
    int? Weight,
    string? Description) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddStudentGradeCommand, Guid>
    {
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IUserService _userService;
        private readonly ISchoolUnitOfWork _unitOfWork;

        public Handler(ISchoolUserRepository schoolUserRepository, ISchoolRepository schoolRepository, IUserService userService, ISchoolUnitOfWork unitOfWork)
        {
            _schoolUserRepository = schoolUserRepository;
            _schoolRepository = schoolRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddStudentGradeCommand request, CancellationToken cancellationToken)
        {
            var user = await _schoolUserRepository.GetTeacherByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            var @class = school.Classes.FirstOrDefault(x => x.Id == Domain.Schools.Ids.ClassId.From(request.ClassId));
            NullValidator.ValidateNotNull(@class);

            var student = await _schoolUserRepository.GetStudentByIdAsync(request.StudentId, cancellationToken);
            NullValidator.ValidateNotNull(student);

            if (!@class.StudentIds.Contains(student.Id)
                || @class.Lessons.All(x => x.AssignedTeacher != user.Id))
                return Result<Guid>.BadRequest("Student is not in this class");

            var grade = new Grade(request.Grade, request.Percentage, request.Description, request.GradeType, request.Weight);

            student.AddGrade(grade);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Ok(student.Id);
        }
    }
}