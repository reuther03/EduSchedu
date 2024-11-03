using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public record AddStudentCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid ClassId,
    Guid StudentId) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddStudentCommand, Guid>
    {
        private readonly IUserService _userService;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly ISchoolUnitOfWork _unitOfWork;

        public Handler(IUserService userService, ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, ISchoolUnitOfWork unitOfWork)
        {
            _userService = userService;
            _schoolRepository = schoolRepository;
            _schoolUserRepository = schoolUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _schoolUserRepository.GetTeacherByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(teacher);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            var student = await _schoolUserRepository.GetStudentByIdAsync(request.StudentId, cancellationToken);
            NullValidator.ValidateNotNull(student);

            var @class = school.Classes.FirstOrDefault(x => x.Id == Domain.Schools.Ids.ClassId.From(request.ClassId));
            NullValidator.ValidateNotNull(@class);

            if (@class.Lessons.All(x => x.AssignedTeacher != teacher.Id))
                return Result<Guid>.BadRequest("User is not a teacher in this class");

            @class.AddStudent(student.Id);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Ok(@class.Id.Value);
        }
    }
}