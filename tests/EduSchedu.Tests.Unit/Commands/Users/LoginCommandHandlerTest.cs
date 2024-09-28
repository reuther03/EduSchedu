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
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly LoginCommand.Handler _handler;

    public LoginCommandHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _handler = new LoginCommand.Handler(_userRepositoryMock.Object, _jwtProviderMock.Object);
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
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(401, result.StatusCode);
    }

    [Fact]
    public async Task Give_UserWithChangedPassword_Should_Return_Ok()
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
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.StatusCode);
    }
}