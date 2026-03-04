using Application.Abstractions;
using Application.Features.Accounts.Commands.RegisterAccount;
using AuthTests.Resources;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AuthTests.Features.Accounts.Commands.RegisterAccount;

public class RegisterAccountCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly TestAuthDbContext _context;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    
    
    public RegisterAccountCommandHandlerTests()
    {
        DbContextOptionsBuilder<TestAuthDbContext> optionsBuilder = new();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new TestAuthDbContext(optionsBuilder.Options);
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _passwordHasherMock
            .Setup(x => x.Hash(It.IsAny<string>()))
            .Returns("Hashed-password");
    }

    [Fact]
    public async Task RegisterAccount_HappyPath_AccountCreatedSuccessfully()
    {
        // Arrange
        var command = _fixture.Create<RegisterAccountCommand>();

        var handler = new RegisterAccountCommandHandler(
            _context,
            _passwordHasherMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        _context.Accounts.Should().ContainSingle(a => a.Email == command.Email);
    }

    [Fact]
    public async Task RegisterAccount_TwoAccountsWithSameEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var command = _fixture.Create<RegisterAccountCommand>();
        var sameCommand = new RegisterAccountCommand(
            Password: command.Password,
            Email: command.Email,
            Role: command.Role,
            PhoneNumber: command.PhoneNumber
        );
        var commandHandler = new RegisterAccountCommandHandler(
            _context, _passwordHasherMock.Object);

        // Act
        var firstRes = await commandHandler.Handle(command, CancellationToken.None);
        var secondRes = await commandHandler.Handle(sameCommand, CancellationToken.None);

        // Assert
        firstRes.IsSuccess.Should().BeTrue();
        secondRes.IsSuccess.Should().BeFalse();
    }
    
}
