using System.Text.Json.Serialization;
using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.CommandValidators;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;

public record AddExistingUserCommand(
    [property: JsonIgnore]
    Guid SchoolId,
    string UserEmail) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<AddExistingUserCommand, Guid>
    {
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IUserService _userService;
        private readonly ISchoolUnitOfWork _schoolUnitOfWork;


        public Handler(ISchoolUserRepository schoolUserRepository, ISchoolRepository schoolRepository, IUserService userService,
            ISchoolUnitOfWork schoolUnitOfWork)
        {
            _schoolUserRepository = schoolUserRepository;
            _schoolRepository = schoolRepository;
            _userService = userService;
            _schoolUnitOfWork = schoolUnitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddExistingUserCommand request, CancellationToken cancellationToken)
        {
            var admin = _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken).Result;
            NullValidator.ValidateNotNull(admin);

            var school = _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken).Result;
            NullValidator.ValidateNotNull(school);

            if (school.HeadmasterId != admin.Id)
                return Result.BadRequest<Guid>("You are not allowed to add users to this school.");

            var user = await _schoolUserRepository.GetByEmailAsync(new Email(request.UserEmail), cancellationToken);
            NullValidator.ValidateNotNull(user);

            //todo: posprawdzac te metody czy na pewno powinno dodacawac teachera
            school.AddTeacher(user.Id);
            await _schoolUnitOfWork.CommitAsync(cancellationToken);

            return Result.Ok(user.Id.Value);
        }
    }
}