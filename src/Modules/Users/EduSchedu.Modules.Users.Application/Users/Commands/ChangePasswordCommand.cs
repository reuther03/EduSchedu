using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;

namespace EduSchedu.Modules.Users.Application.Users.Commands;

public record ChangePasswordCommand(string Email, string OldPassword, string NewPassword) : ICommand
{
    internal sealed class Handler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null)
                return Result.NotFound("User not found");

            if (!user.Password.Verify(request.OldPassword))
                return Result.Unauthorized("Old password is invalid");

            user.ChangePassword(request.NewPassword);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Ok();
        }
    }
}