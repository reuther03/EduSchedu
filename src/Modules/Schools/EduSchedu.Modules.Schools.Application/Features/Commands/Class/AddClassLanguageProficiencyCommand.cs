using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Class;

public record AddClassLanguageProficiencyCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    [property: JsonIgnore]
    Guid ClassId,
    Guid LanguageProficiencyId
) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddClassLanguageProficiencyCommand, Guid>
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly ILanguageProficiencyRepository _languageProficiencyRepository;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly IUserService _userService;
        private readonly ISchoolUnitOfWork _unitOfWork;

        public Handler(
            ISchoolRepository schoolRepository,
            ILanguageProficiencyRepository languageProficiencyRepository,
            ISchoolUserRepository schoolUserRepository,
            IUserService userService,
            ISchoolUnitOfWork unitOfWork
        )
        {
            _schoolRepository = schoolRepository;
            _languageProficiencyRepository = languageProficiencyRepository;
            _schoolUserRepository = schoolUserRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddClassLanguageProficiencyCommand request, CancellationToken cancellationToken)
        {
            var user = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            NullValidator.ValidateNotNull(user);

            if (user.Role == Role.Teacher)
                return Result<Guid>.BadRequest("You are not allowed to add language proficiency");

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            var @class = await _schoolRepository.GetClassByIdAsync(request.SchoolId, request.ClassId, cancellationToken);
            NullValidator.ValidateNotNull(@class);

            var languageProficiency = await _languageProficiencyRepository.GetByIdAsync(request.LanguageProficiencyId, cancellationToken);
            NullValidator.ValidateNotNull(languageProficiency);

            @class.SetLanguageProficiency(languageProficiency);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(@class.Id.Value);
        }
    }
}