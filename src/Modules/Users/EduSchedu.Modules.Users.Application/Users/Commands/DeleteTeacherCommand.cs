using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;

namespace EduSchedu.Modules.Users.Application.Users.Commands;

public record DeleteTeacherCommand(Guid TeacherId) : ICommand
{
    internal sealed class Handler : ICommandHandler<DeleteTeacherCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUnitOfWork _unitOfWork;

        public Handler(IUserRepository userRepository, IUserUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = await _userRepository.GetByIdAsync(request.TeacherId, cancellationToken);
            if (teacher is null)
            {
                return Result.BadRequest($"Teacher not found.");
            }

            _userRepository.Remove(teacher);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Ok();
        }
    }
}