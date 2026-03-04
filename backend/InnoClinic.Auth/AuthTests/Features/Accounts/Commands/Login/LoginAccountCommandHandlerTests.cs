using Application.Abstractions;
using Application.Features.Accounts.Commands.Login;
using AuthTests.Resources;
using AutoFixture;
using Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AuthTests.Features.Accounts.Commands.Login;

public class LoginAccountCommandHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IHashProvider> _hashProviderMock;
    private readonly TestAuthDbContext _dbContext;
    
    public LoginAccountCommandHandlerTests()
    {
        _fixture = new Fixture();
        DbContextOptionsBuilder<TestAuthDbContext> builder =  new DbContextOptionsBuilder<TestAuthDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dbContext = new TestAuthDbContext(builder.Options);
        
        _passwordHasherMock =  new Mock<IPasswordHasher>();
        _passwordHasherMock
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns("hashed");
        _passwordHasherMock
            .Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true); // Tell the mock that the password ALWAYS matches
        
        _jwtProviderMock = new Mock<IJwtProvider>();
        _jwtProviderMock
            .Setup(x => x.GenerateJwtToken(It.IsAny<Account>()))
            .Returns("access");
        
        _jwtProviderMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns(_fixture.Create<string>());
        
        
        _hashProviderMock = new Mock<IHashProvider>();
        _hashProviderMock
            .Setup(x => x.GenerateHash(It.IsAny<string>()))
            .Returns("hashed");
    }

    [Theory]
    [InlineData("email@email.com", "Qwerty123")]
    public async Task LoginAccount_ValidData_HasNoErrors(string email, string psw)
    {
        // Arrange 
        var account = new Account(
            email: email,
            passwordHash: "hashed_in_db",
            phoneNumber: "1234567890",
            role: default
            );
        
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        _passwordHasherMock
            .Setup(x => x.Verify(psw, "hashed_in_db")) 
            .Returns(true);

        var loginCommand = new LoginUserCommand(email, psw);
    
        var loginCommandHandler = new LoginUserCommandHandler(
            context: _dbContext,
            passwordHasher: _passwordHasherMock.Object,
            jwtProvider: _jwtProviderMock.Object,
            hashProvider: _hashProviderMock.Object
        );

        // Act
        var res = await loginCommandHandler.Handle(loginCommand, CancellationToken.None);

        // Assert
        res.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task Handle_UserNotFound_ReturnsFailure()
    {
        // Arrange
        var loginCommand = new LoginUserCommand("nonexistent@test.com", "any_password");
        var loginCommandHandler = new LoginUserCommandHandler(
            _dbContext, 
            _passwordHasherMock.Object, 
            _jwtProviderMock.Object, 
            _hashProviderMock.Object
        );

        // Act
        var res = await loginCommandHandler.Handle(loginCommand, CancellationToken.None);

        // Assert
        res.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WrongPassword_ReturnsFailure()
    {
        // Arrange
        var email = "user@test.com";
        var password = "Wrong_password1";
        var dbHash = "secure_hash";
    
        var account = new Account(
            email: email,
            passwordHash: dbHash,
            phoneNumber: "0000000000",
            role: default
        );

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        _passwordHasherMock
            .Setup(x => x.Verify(password, dbHash))
            .Returns(false);

        var loginCommand = new LoginUserCommand(email, password);
        var loginCommandHandler = new LoginUserCommandHandler(
            _dbContext, 
            _passwordHasherMock.Object, 
            _jwtProviderMock.Object, 
            _hashProviderMock.Object
        );

        // Act
        var res = await loginCommandHandler.Handle(loginCommand, CancellationToken.None);

        // Assert
        res.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenEmailIsEmpty_ReturnsFailure()
    {
        // Arrange
        var loginCommand = new LoginUserCommand(string.Empty, "Any_password1");
        var loginCommandHandler = new LoginUserCommandHandler(
            _dbContext, 
            _passwordHasherMock.Object, 
            _jwtProviderMock.Object, 
            _hashProviderMock.Object
        );

        // Act
        var res = await loginCommandHandler.Handle(loginCommand, CancellationToken.None);

        // Assert
        res.IsSuccess.Should().BeFalse();
    }
    
}
