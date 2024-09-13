using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddTeacherLanguageProficiencyCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    Guid TeacherId,
    Guid LanguageProficiencyId) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddTeacherLanguageProficiencyCommand, Guid>
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

        public async Task<Result<Guid>> Handle(AddTeacherLanguageProficiencyCommand request, CancellationToken cancellationToken)
        {
            var user = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            if (user is null)
                return Result<Guid>.BadRequest("User not found");

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            if (school is null)
                return Result<Guid>.BadRequest("School not found");

            if (user.Role == Role.Teacher && !school.TeacherIds.Contains(user.Id))
                return Result<Guid>.BadRequest("You are not allowed to add language proficiency");

            var teacher = await _schoolUserRepository.GetTeacherByIdAsync(request.TeacherId, cancellationToken);
            if (teacher is null)
                return Result<Guid>.BadRequest("Teacher not found");

            if (!school.TeacherIds.Contains(teacher.Id))
                return Result<Guid>.BadRequest("Teacher is not in the school");

            var languageProficiency = await _languageProficiencyRepository.GetByIdAsync(request.LanguageProficiencyId, cancellationToken);
            if (languageProficiency is null || teacher.LanguageProficiencyIds.Any(x => x.Value == languageProficiency.Id))
                return Result<Guid>.BadRequest("Language proficiency not found or already exists");

            foreach (var languageProficiencyId in teacher.LanguageProficiencyIds.ToList())
            {
                var existingLanguageProficiency = await _languageProficiencyRepository.GetByIdAsync(languageProficiencyId, cancellationToken);
                if (existingLanguageProficiency!.Language == languageProficiency.Language && existingLanguageProficiency.Lvl < languageProficiency.Lvl)
                {
                    teacher.RemoveLanguageProficiency(existingLanguageProficiency.Id);
                    teacher.AddLanguageProficiency(languageProficiency.Id);
                }
                else if (existingLanguageProficiency.Language != languageProficiency.Language)
                {
                    teacher.AddLanguageProficiency(languageProficiency.Id);
                }

                //todo: czy to na pewno ma byc w petli, ale inaczej nie dziala xd
                await _unitOfWork.CommitAsync(cancellationToken);
            }

            return Result<Guid>.Ok(teacher.Id.Value);
        }
    }
}