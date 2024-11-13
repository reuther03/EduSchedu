using System.Text.Json.Serialization;
using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Email;
using EduSchedu.Shared.Abstractions.Integration.Events.Users;
using EduSchedu.Shared.Abstractions.Kernel.Primitives;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using EduSchedu.Shared.Abstractions.QueriesAndCommands.Commands;
using EduSchedu.Shared.Abstractions.Services;
using UserPassword = EduSchedu.Modules.Users.Domain.Users.Password;
using MediatR;

namespace EduSchedu.Modules.Users.Application.Users.Commands;

public record CreateUserCommand(
    string Email,
    string FullName,
    Role Role,
    [property: JsonIgnore]
    Guid SchoolId) : ICommand<Guid>
{
internal sealed class Handler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPublisher _publisher;
    private readonly IUserService _userService;
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWork _unitOfWork;

    public Handler(IUserRepository userRepository, IPublisher publisher, IUserService userService, IEmailSender emailSender, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _publisher = publisher;
        _userService = userService;
        _emailSender = emailSender;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var headmaster = await _userRepository.GetByIdAsync(_userService.UserId, cancellationToken);
        if (headmaster is null || headmaster.Role != Role.HeadMaster)
            return Result.NotFound<Guid>("Headmaster not found.");

        if (_userRepository.ExistsWithEmailAsync(request.Email, cancellationToken).Result)
            return Result.BadRequest<Guid>("User with this email already exists.");

        if (request.Role is Role.HeadMaster or Role.BackOffice)
            return Result.BadRequest<Guid>("Headmaster can't be created here.");

        var password = UserPassword.Generate();

        var user = User.Create(new Email(request.Email), new Name(request.FullName), UserPassword.Create(password), request.Role);

        var email = new EmailMessage(user.Email,
            "Your EduSchedu Account Details",
            $"""
             <div style="background-color: #f0f0f0; padding: 20px; font-family: Arial, sans-serif; line-height: 1.6;">
                 <div style="max-width: 600px; margin: auto; background: #ffffff; padding: 20px; border-radius: 8px;">
                     <h2 style="color: #333333; text-align: center;">Welcome to EduSchedu!</h2>
                     <p style="color: #555555;">Hello {user.FullName},</p>
                     <p style="color: #555555;">Your account has been successfully created. Below are your login details:</p>
                     <p style="color: #555555;"><strong>Email:</strong> {user.Email}</p>
                     <p style="color: #555555;"><strong>Password:</strong> {password}</p>
                     <p style="color: #555555;">Please ensure to change your password before first login for security purposes.</p>
                     <div style="text-align: center; margin-top: 20px;">
                         <a href="https://eduschedu.com/login" style="display: inline-block; background-color: #007bff; color: #ffffff; padding: 10px 20px; border-radius: 5px; text-decoration: none;">Login to EduSchedu</a>
                     </div>
                     <p style="color: #555555; margin-top: 20px;">If you have any questions or need assistance, please reach out to our support team.</p>
                     <div style="text-align: center; margin-top: 20px;">
                         <a href="https://support.eduschedu.com" style="display: inline-block; background-color: #007bff; color: #ffffff; padding: 10px 20px; border-radius: 5px; text-decoration: none;">Contact Support</a>
                     </div>
                 </div>
             </div>
             """);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        await _emailSender.Send(email);

        await _publisher.Publish(new UserCreatedEvent(user.Id, user.FullName, user.Email, user.Role, request.SchoolId), cancellationToken);

        return Result.Ok(user.Id.Value);
    }
}
}