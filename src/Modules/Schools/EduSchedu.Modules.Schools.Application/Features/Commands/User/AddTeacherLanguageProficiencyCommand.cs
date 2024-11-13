﻿using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
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
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            ISchoolRepository schoolRepository,
            ILanguageProficiencyRepository languageProficiencyRepository,
            ISchoolUserRepository schoolUserRepository,
            IUserService userService,
            IUnitOfWork unitOfWork
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
            NullValidator.ValidateNotNull(user);

            var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
            NullValidator.ValidateNotNull(school);

            if (user.Role == Role.Teacher && !school.TeacherIds.Contains(user.Id))
                return Result<Guid>.BadRequest("You are not allowed to add language proficiency");

            var teacher = await _schoolUserRepository.GetTeacherByIdAsync(request.TeacherId, cancellationToken);
            NullValidator.ValidateNotNull(teacher);

            if (!school.TeacherIds.Contains(teacher.Id))
                return Result<Guid>.BadRequest("Teacher is not in the school");

            var languageProficiency = await _languageProficiencyRepository.GetByIdAsync(request.LanguageProficiencyId, cancellationToken);
            if (languageProficiency is null || teacher.LanguageProficiencyIds.Any(x => x.Value == languageProficiency.Id))
                return Result<Guid>.BadRequest("Language proficiency not found or already exists");

            if (teacher.LanguageProficiencyIds.Count == 0)
            {
                teacher.AddLanguageProficiency(languageProficiency.Id);
                await _unitOfWork.CommitAsync(cancellationToken);
                return Result<Guid>.Ok(languageProficiency.Id);
            }

            foreach (var languageProficiencyId in teacher.LanguageProficiencyIds.ToList())
            {
                var existingLanguageProficiency = await _languageProficiencyRepository.GetByIdAsync(languageProficiencyId, cancellationToken);
                //todo: check if this is correct
                NullValidator.ValidateNotNull(existingLanguageProficiency);

                if (existingLanguageProficiency.Language != languageProficiency.Language
                    && teacher.LanguageProficiencyIds.All(x => x.Value != languageProficiency.Id))
                {
                    teacher.AddLanguageProficiency(languageProficiency.Id);
                }

                else if (existingLanguageProficiency.Language == languageProficiency.Language && existingLanguageProficiency.Lvl != languageProficiency.Lvl)
                {
                    teacher.RemoveLanguageProficiency(existingLanguageProficiency.Id);
                    teacher.AddLanguageProficiency(languageProficiency.Id);
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(languageProficiency.Id);
        }
    }
}