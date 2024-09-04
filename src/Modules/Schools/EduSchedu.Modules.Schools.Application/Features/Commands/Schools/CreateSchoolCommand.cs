using EduSchedu.Modules.Schools.Application.Abstractions;
using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Schools.Domain.Schools;
using EduSchedu.Modules.Schools.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;

namespace EduSchedu.Modules.Schools.Application.Features.Commands.Schools;

public record CreateSchoolCommand(
    string Name,
    string City,
    string Street,
    string ZipCode,
    string? MapCoordinates,
    string PhoneNumber,
    string Email
) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<CreateSchoolCommand, Guid>
    {
        private readonly IUserService _userService;
        private readonly ISchoolUserRepository _schoolUserRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly ISchoolUnitOfWork _schoolUnitOfWork;

        public Handler(IUserService userService, ISchoolUserRepository schoolUserRepository, ISchoolRepository schoolRepository,
            ISchoolUnitOfWork schoolUnitOfWork)
        {
            _userService = userService;
            _schoolUserRepository = schoolUserRepository;
            _schoolRepository = schoolRepository;
            _schoolUnitOfWork = schoolUnitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var headmaster = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            if (headmaster is null)
                return Result<Guid>.BadRequest("HeadMaster not found");

            if (headmaster.Role != Role.HeadMaster && headmaster.Role != Role.BackOffice)
                return Result<Guid>.BadRequest("User is not a principal or back office");

            var school = School.Create(
                request.Name,
                new Address(request.City, request.Street, request.ZipCode, request.MapCoordinates),
                request.PhoneNumber,
                request.Email,
                headmaster.Id
            );

            school.AddUser(headmaster.Id);
            await _schoolRepository.AddAsync(school, cancellationToken);
            await _schoolUnitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Ok(school.Id.Value);
        }
    }
}