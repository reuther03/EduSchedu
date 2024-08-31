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

public record SignUpCommand(string Email, string FullName, string Password) : ICommand<Guid>
{
    internal sealed class Handler : ICommandHandler<SignUpCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublisher _publisher;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUserRepository userRepository, IPublisher publisher, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _publisher = publisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            if (_userRepository.ExistsWithEmailAsync(request.Email, cancellationToken).Result)
                return Result.BadRequest<Guid>("User with this email already exists.");

            var user = User.Create(new Email(request.Email), new Name(request.FullName), UserPassword.Create(request.Password));

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            await _publisher.Publish(new UserCreatedEvent(user.Id, user.FullName, user.Email), cancellationToken);

            return Result.Ok(user.Id.Value);
        }
    }
}