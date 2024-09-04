using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Events;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;
using UserPassword = EduSchedu.Modules.Users.Domain.Users.Password;
using MediatR;

namespace EduSchedu.Modules.Users.Application.Users.Commands;

public record CreateUserCommand(string Email, string FullName, string Password, Role Role, Guid SchoolId) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublisher _publisher;
        private readonly IUserService _userService;
        private readonly IUserUnitOfWork _userUnitOfWork;

        public Handler(IUserRepository userRepository, IPublisher publisher, IUserService userService, IUserUnitOfWork userUnitOfWork)
        {
            _userRepository = userRepository;
            _publisher = publisher;
            _userService = userService;
            _userUnitOfWork = userUnitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var headmaster = await _userRepository.GetByIdAsync(_userService.UserId, cancellationToken);
            if (headmaster is null || headmaster.Role != Role.HeadMaster)
                return Result.NotFound<Guid>("Headmaster not found.");

            if (_userRepository.ExistsWithEmailAsync(request.Email, cancellationToken).Result)
                return Result.BadRequest<Guid>("User with this email already exists.");

            if (request.Role == Role.HeadMaster)
                return Result.BadRequest<Guid>("Headmaster can't be created here.");

            var user = User.Create(new Email(request.Email), new Name(request.FullName), UserPassword.Create(request.Password), request.Role);

            await _userRepository.AddAsync(user, cancellationToken);
            await _userUnitOfWork.CommitAsync(cancellationToken);

            await _publisher.Publish(new UserCreatedEvent(user.Id, user.FullName, user.Email, user.Role, request.SchoolId), cancellationToken);

            return Result.Ok(user.Id.Value);
        }
    }
}