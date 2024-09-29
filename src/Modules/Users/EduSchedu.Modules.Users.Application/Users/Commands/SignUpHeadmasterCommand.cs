using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Events;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using MediatR;
using UserPassword = EduSchedu.Modules.Users.Domain.Users.Password;

namespace EduSchedu.Modules.Users.Application.Users.Commands;

public record SignUpHeadmasterCommand(string Email, string FullName, string Password) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<SignUpHeadmasterCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublisher _publisher;
        private readonly IUserUnitOfWork _userUnitOfWork;

        public Handler(IUserRepository userRepository, IPublisher publisher, IUserUnitOfWork userUnitOfWork)
        {
            _userRepository = userRepository;
            _publisher = publisher;
            _userUnitOfWork = userUnitOfWork;
        }

        public async Task<Result<Guid>> Handle(SignUpHeadmasterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsWithEmailAsync(request.Email, cancellationToken))
                return Result.BadRequest<Guid>("User with this email already exists.");

            var user = User.Create(new Email(request.Email), new Name(request.FullName), UserPassword.Create(request.Password), Role.HeadMaster);

            await _userRepository.AddAsync(user, cancellationToken);
            await _userUnitOfWork.CommitAsync(cancellationToken);

            await _publisher.Publish(new HeadmasterCreatedEvent(user.Id, user.FullName, user.Email), cancellationToken);

            return Result.Ok(user.Id.Value);
        }
    }
}