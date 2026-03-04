using Application.Abstractions;
using Application.Features.Accounts.Commands.RefreshTokens;
using AuthTests.Resources;
using Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AuthTests.Features.Accounts.Commands.Refresh;

public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IHashProvider> _hashProviderMock;
    private readonly TestAuthDbContext _dbContext;

    public RefreshTokenCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestAuthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TestAuthDbContext(options);
        _jwtProviderMock = new Mock<IJwtProvider>();
        _hashProviderMock = new Mock<IHashProvider>();
    }

    [Fact]
    public async Task Handle_InvalidToken_ReturnsFailure()
    {
        // Arrange
        var rawToken = "invalid-token";
        var hashedToken = "hashed-invalid";
        _hashProviderMock.Setup(x => x.GenerateHash(rawToken)).Returns(hashedToken);

        var command = new RefreshTokenCommand("access", rawToken);
        var handler = new RefreshTokenCommandHandler(_dbContext, _jwtProviderMock.Object, _hashProviderMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_TokenIsRevoked_ReturnsFailure()
    {
        // Arrange
        var rawToken = "revoked-token";
        var hashedToken = "hashed-revoked";
        _hashProviderMock.Setup(x => x.GenerateHash(rawToken)).Returns(hashedToken);

        var refreshToken = new Domain.Models.RefreshToken(Guid.NewGuid(), hashedToken, DateTime.UtcNow.AddDays(1));
        refreshToken.Revoke("Test reason");
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        var command = new RefreshTokenCommand("access", rawToken);
        var handler = new RefreshTokenCommandHandler(_dbContext, _jwtProviderMock.Object, _hashProviderMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_TokenIsExpired_ReturnsFailure()
    {
        // Arrange
        var rawToken = "expired-token";
        var hashedToken = "hashed-expired";
        _hashProviderMock.Setup(x => x.GenerateHash(rawToken)).Returns(hashedToken);

        var refreshToken = new Domain.Models.RefreshToken(Guid.NewGuid(), hashedToken, DateTime.UtcNow.AddDays(-1));
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        var command = new RefreshTokenCommand("access", rawToken);
        var handler = new RefreshTokenCommandHandler(_dbContext, _jwtProviderMock.Object, _hashProviderMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_AccountNotFound_ReturnsFailure()
    {
        // Arrange
        var rawToken = "valid-token";
        var hashedToken = "hashed-valid";
        _hashProviderMock.Setup(x => x.GenerateHash(rawToken)).Returns(hashedToken);

        var refreshToken = new Domain.Models.RefreshToken(Guid.NewGuid(), hashedToken, DateTime.UtcNow.AddDays(1));
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        var command = new RefreshTokenCommand("access", rawToken);
        var handler = new RefreshTokenCommandHandler(_dbContext, _jwtProviderMock.Object, _hashProviderMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessAndRotatesTokens()
    {
        // Arrange
        var account = new Account("user@test.com", "db_hash", default, "1234567890");
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        var oldRawToken = "valid-old-token";
        var oldHashedToken = "hashed-old";
        var newRawToken = "new-refresh-token";
        var newHashedToken = "hashed-new";

        var refreshToken = new Domain.Models.RefreshToken(account.Id, oldHashedToken, DateTime.UtcNow.AddDays(1));
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        _hashProviderMock.Setup(x => x.GenerateHash(oldRawToken)).Returns(oldHashedToken);
        _hashProviderMock.Setup(x => x.GenerateHash(newRawToken)).Returns(newHashedToken);
        _jwtProviderMock.Setup(x => x.GenerateRefreshToken()).Returns(newRawToken);
        _jwtProviderMock.Setup(x => x.GenerateJwtToken(It.IsAny<Account>())).Returns("new-access-token");

        var command = new RefreshTokenCommand("old-access", oldRawToken);
        var handler = new RefreshTokenCommandHandler(_dbContext, _jwtProviderMock.Object, _hashProviderMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.RefreshToken.Should().Be(newRawToken);
        refreshToken.IsRevoked.Should().BeTrue();
        
        var savedNewToken = await _dbContext.RefreshTokens
            .AnyAsync(t => t.TokenHash == newHashedToken && t.AccountId == account.Id);
        savedNewToken.Should().BeTrue();
    }
}
