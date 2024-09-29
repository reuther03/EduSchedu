using EduSchedu.Modules.Users.Application.Abstractions;
using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Users.Application.Users.Commands;
using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using FluentAssertions;
using MediatR;
using Moq;

namespace EduSchedu.Tests.Unit.Commands.Users;

public class SignUpCommandHandlerTest
{
    private const string UserPassword = "testtest";
    private readonly User _user = User.Create(new Email("test@emial.com"), new Name("test name"), Password.Create(UserPassword), Role.HeadMaster);
    private SignUpHeadmasterCommand CreateCommand() => new(_user.Email.Value, _user.FullName.Value, UserPassword);

    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPublisher> _publisher = new();
    private readonly Mock<IUserUnitOfWork> _unitOfWork = new();


    [Fact]
    public async Task Give_User_With_Existing_Email()
    {
        _userRepositoryMock.Setup(x => x.ExistsWithEmailAsync(_user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new SignUpHeadmasterCommand.Handler(_userRepositoryMock.Object, _publisher.Object, _unitOfWork.Object);
        var command = CreateCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _publisher.Verify(x => x.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Give_User_With_Valid_Data()
    {
        _userRepositoryMock.Setup(x => x.ExistsWithEmailAsync(_user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userRepositoryMock.Setup(x => x.AddAsync(_user, It.IsAny<CancellationToken>()));
        _unitOfWork.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()));
        _publisher.Setup(x => x.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));

        var handler = new SignUpHeadmasterCommand.Handler(_userRepositoryMock.Object, _publisher.Object, _unitOfWork.Object);
        var command = CreateCommand();

        var result = await handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _publisher.Verify(x => x.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);
    }
}