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
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUserService userService, ISchoolUserRepository schoolUserRepository, ISchoolRepository schoolRepository, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _schoolUserRepository = schoolUserRepository;
            _schoolRepository = schoolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var principal = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            if (principal is null)
                return Result<Guid>.BadRequest("Principal not found");

            if (principal.Role != Role.Principal && principal.Role != Role.BackOffice)
                return Result<Guid>.BadRequest("User is not a principal or back office");

            var school = School.Create(
                request.Name,
                new Address(request.City, request.Street, request.ZipCode, request.MapCoordinates),
                request.PhoneNumber,
                request.Email,
                principal.Id
            );

            await _schoolRepository.AddAsync(school, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result<Guid>.Ok(school.Id.Value);
        }
    }
}