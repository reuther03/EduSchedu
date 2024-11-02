using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Shared.Abstractions.Auth;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;

namespace EduSchedu.Modules.Users.Application.Users.Commands;

public record LoginCommand(string Email, string Password) : ICommand<AccessToken>
{
    internal sealed class Handler : ICommandHandler<LoginCommand, AccessToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public Handler(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<AccessToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null)
                return Result.Unauthorized<AccessToken>("Authentication failed");

            if (!user.IsPasswordChanged && user.Role != Role.HeadMaster)
                return Result.Unauthorized<AccessToken>("You must change your password");

            if (!user.Password.Verify(request.Password))
                return Result.Unauthorized<AccessToken>("Authentication failed");

            var token = AccessToken.Create(
                _jwtProvider.GenerateToken(user.Id.ToString(), user.FullName, user.Email, user.Role),
                user.Id,
                user.FullName,
                user.Email
            );

            return Result.Ok(token);
        }
    }
}