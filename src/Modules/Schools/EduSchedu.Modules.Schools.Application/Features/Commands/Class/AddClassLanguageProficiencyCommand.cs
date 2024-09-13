using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Application.Features.Commands.User;
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
            if (user is null)
                return Result<Guid>.BadRequest("User not found");

            if (user.Role == Role.Teacher)
                return Result<Guid>.BadRequest("You are not allowed to add language proficiency");

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            if (school is null)
                return Result<Guid>.BadRequest("School not found");

            var @class = await _schoolRepository.GetClassByIdAsync(request.SchoolId, request.ClassId, cancellationToken);
            if (@class is null)
                return Result<Guid>.BadRequest("Class not found");

            var languageProficiency = await _languageProficiencyRepository.GetByIdAsync(request.LanguageProficiencyId, cancellationToken);
            if (languageProficiency is null || @class.LanguageProficiencyIds.Any(x => x.Value == languageProficiency.Id))
                return Result<Guid>.BadRequest("Language proficiency not found or already exists");

            if (@class.LanguageProficiencyIds.Count == 0)
            {
                @class.AddLanguageProficiency(languageProficiency.Id);
                await _unitOfWork.CommitAsync(cancellationToken);
                return Result<Guid>.Ok(languageProficiency.Id);
            }

            foreach (var languageProficiencyId in @class.LanguageProficiencyIds.ToList())
            {
                var existingLanguageProficiency = await _languageProficiencyRepository.GetByIdAsync(languageProficiencyId, cancellationToken);
                if (existingLanguageProficiency!.Language != languageProficiency.Language ||
                    existingLanguageProficiency.Lvl >= languageProficiency.Lvl)
                    Result<Guid>.BadRequest("Language proficiency already exists");


                @class.RemoveLanguageProficiency(existingLanguageProficiency.Id);
                @class.AddLanguageProficiency(languageProficiency.Id);
                await _unitOfWork.CommitAsync(cancellationToken);
            }

            return Result.Ok(languageProficiency.Id);
        }
    }
}