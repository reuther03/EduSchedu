// using EduSchedu.Modules.Schools.Application.Abstractions;
// using EduSchedu.Modules.Schools.Application.Abstractions.Database.Repositories;
// using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
// using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
// using EduSchedu.Shared.Abstractions.Services;
//
// namespace EduSchedu.Modules.Schools.Application.Features.Commands.User;
//
// public class CreateBackofficeUserCommand : ICommand<Guid>
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
//         }
//     }
// }