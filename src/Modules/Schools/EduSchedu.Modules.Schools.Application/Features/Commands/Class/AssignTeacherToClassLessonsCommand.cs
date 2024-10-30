using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public class AssignTeacherToClassLessonsCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    IReadOnlyList<Guid> LessonIds) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AssignTeacherToClassLessonsCommand, Guid>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUserService _userService;
        private readonly ISchoolUnitOfWork _unitOfWork;

        public Handler(ISchoolRepository schoolRepository, ISchoolUserRepository schoolUserRepository, IUserService userService, ISchoolUnitOfWork unitOfWork)
        {
            _schoolRepository = schoolRepository;
            _schoolUserRepository = schoolUserRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public Task<Result<Guid>> Handle(AssignTeacherToClassLessonsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}