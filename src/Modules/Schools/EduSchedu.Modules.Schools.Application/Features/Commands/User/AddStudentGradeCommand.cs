using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Users.Students;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddStudentGradeCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid StudentId,
    float Grade,
    int Percentage,
    GradeType GradeType,
    int Weight,
    string Description) : ICommand<Guid>
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
            var user = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            var student = await _schoolUserRepository.GetStudentByIdAsync(request.StudentId, cancellationToken);
            NullValidator.ValidateNotNull(student);

            if (!school.StudentIds.Contains(student.Id))
                return Result<Guid>.BadRequest("Student not found in school");

            var grade = new Grade(request.Grade, request.Percentage, request.Description, request.GradeType, request.Weight);
            //todo: walidacja czy uczen i user ktory dodaje ocene sa w tej samej klasie

            student.AddGrade(grade);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Ok(student.Id);
        }
    }
}