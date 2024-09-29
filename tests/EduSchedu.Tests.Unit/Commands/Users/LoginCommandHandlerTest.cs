using EduSchedu.Modules.Users.Application.Abstractions.Database.Repositories;
using EduSchedu.Modules.Users.Application.Users.Commands;
using EduSchedu.Modules.Users.Domain.Users;
using EduSchedu.Shared.Abstractions.Auth;
using EduSchedu.Shared.Abstractions.Kernel.Primitives.Result;
using EduSchedu.Shared.Abstractions.Kernel.ValueObjects;
using FluentAssertions;
using Moq;

namespace EduSchedu.Tests.Unit.Commands.Users;

public class LoginCommandHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IJwtProvider> _jwtProviderMock = new();

    [Fact]
    public async Task Give_User_With_Invalid_Email_Should_Return_Unauthorized()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new LoginCommand.Handler(_userRepositoryMock.Object, _jwtProviderMock.Object);
        var command = new LoginCommand("test@testemail.com", "testpassword");

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(401);
    }


    [Fact]
    public async Task Give_UserWithoutChangedPassword_Should_Return_Unauthorized()
    {
        // Arrange
        var user = User.Create("testemail@test.com", "Test User", Password.Create("testpassword"), Role.Teacher);

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new LoginCommand.Handler(_userRepositoryMock.Object, _jwtProviderMock.Object);
        var command = new LoginCommand(user.Email, "testpassword");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task Give_User_With_Changed_Password_Should_Return_Ok()
    {
        // Arrange
        var user = User.Create("testemail@test.com", "Test User", Password.Create("testpassword"), Role.Teacher);
        user.ChangePassword("newpassword");

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new LoginCommand.Handler(_userRepositoryMock.Object, _jwtProviderMock.Object);
        var command = new LoginCommand(user.Email, "newpassword");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);
    }
}