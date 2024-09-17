// using EduSchedu.Modules.School.Application.Abstractions;
// using EduSchedu.Modules.School.Application.Abstractions.Database.Repositories;
// using EduSchedu.Modules.School.Domain.Users;
// using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
// using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
// using EduSchedu.Shared.Abstractions.Services;
//
// namespace EduSchedu.Modules.School.Application.Features.Commands.User;
//
// public record CreateBackofficeUserCommand(Guid SchoolId) : ICommand<Guid>
// {
//     internal sealed class Handler : ICommandHandler<CreateBackofficeUserCommand, Guid>
//     {
//         private readonly ISchoolUserRepository _schoolUserRepository;
//         private readonly ISchoolRepository _schoolRepository;
//         private readonly IUserService _userService;
//         private readonly ISchoolUnitOfWork _unitOfWork;
//
//         public Handler(ISchoolUserRepository schoolUserRepository, ISchoolRepository schoolRepository, IUserService userService, ISchoolUnitOfWork unitOfWork)
//         {
//             _schoolUserRepository = schoolUserRepository;
//             _schoolRepository = schoolRepository;
//             _userService = userService;
//             _unitOfWork = unitOfWork;
//         }
//
//         public async Task<Result<Guid>> Handle(CreateBackofficeUserCommand request, CancellationToken cancellationToken)
//         {
//             var principal = await _schoolUserRepository.GetByIdAsync(_userService.UserId, cancellationToken);
//             if (principal is null)
//                 return Result.NotFound<Guid>("User not found.");
//
//             var school = await _schoolRepository.GetByIdAsync(request.SchoolId, cancellationToken);
//             if (school is null)
//                 return Result.NotFound<Guid>("School not found.");
//
//             var user = BackOfficeUser.Create()
//         }
//     }
// }