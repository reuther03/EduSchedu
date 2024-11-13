using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public record CreateClassCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    Guid LanguageProficiencyId,
    string ClassName) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<CreateClassCommand, Guid>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly ILanguageProficiencyRepository _languageProficiencyRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            ISchoolRepository schoolRepository,
            ISchoolUserRepository schoolUserRepository,
            ILanguageProficiencyRepository languageProficiencyRepository,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            _schoolRepository = schoolRepository;
            _schoolUserRepository = schoolUserRepository;
            _languageProficiencyRepository = languageProficiencyRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            var admin = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(admin);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            if (admin.Role == Role.Teacher && !school.TeacherIds.Contains(admin.Id))
                return Result<Guid>.BadRequest("You are not allowed to create class");

            var languageProficiency = await _languageProficiencyRepository.GetByIdAsync(request.LanguageProficiencyId, cancellationToken);
            NullValidator.ValidateNotNull(languageProficiency);

            var @class = Domain.Schools.Class.Create(request.ClassName, languageProficiency);

            school.AddClass(@class);
            await _schoolRepository.AddClassAsync(@class, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Ok(@class.Id.Value);
        }
    }
}