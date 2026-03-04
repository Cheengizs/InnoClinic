using Application.Abstractions;
using Application.Features.Accounts.Commands.Revoke;
using AuthTests.Resources;
using Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AuthTests.Features.Accounts.Commands.Revoke;

public class RevokeTokenCommandHandlerTests
{
    private readonly Mock<IHashProvider> _hashProviderMock;
    private readonly TestAuthDbContext _dbContext;

    public RevokeTokenCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestAuthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TestAuthDbContext(options);
        _hashProviderMock = new Mock<IHashProvider>();
    }

    [Fact]
    public async Task Handle_TokenNotFound_ReturnsFailure()
    {
        // Arrange
        var rawToken = "missing-token";
        var hashedToken = "hashed-missing";
        _hashProviderMock.Setup(x => x.GenerateHash(rawToken)).Returns(hashedToken);

        var command = new RevokeTokenCommand(rawToken);
        var handler = new RevokeTokenCommandHandler(_dbContext, _hashProviderMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_TokenIsActive_RevokesTokenAndReturnsSuccess()
    {
        // Arrange
        var rawToken = "active-token";
        var hashedToken = "hashed-active";
        _hashProviderMock.Setup(x => x.GenerateHash(rawToken)).Returns(hashedToken);

        var refreshToken = new RefreshToken(
            accountId: Guid.NewGuid(), 
            tokenHash: hashedToken, 
            expiresAt: DateTime.UtcNow.AddDays(1)
        );
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        var command = new RevokeTokenCommand(rawToken);
        var handler = new RevokeTokenCommandHandler(_dbContext, _hashProviderMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        refreshToken.IsRevoked.Should().BeTrue();
        
        var updatedToken = await _dbContext.RefreshTokens.FirstAsync(x => x.TokenHash == hashedToken);
        updatedToken.IsRevoked.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_TokenIsAlreadyRevoked_ReturnsSuccessWithoutChanges()
    {
        // Arrange
        var rawToken = "already-revoked-token";
        var hashedToken = "hashed-revoked";
        _hashProviderMock.Setup(x => x.GenerateHash(rawToken)).Returns(hashedToken);

        var refreshToken = new RefreshToken(
            accountId: Guid.NewGuid(), 
            tokenHash: hashedToken, 
            expiresAt: DateTime.UtcNow.AddDays(1)
        );
        refreshToken.Revoke("Initial revocation");
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        var command = new RevokeTokenCommand(rawToken);
        var handler = new RevokeTokenCommandHandler(_dbContext, _hashProviderMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        refreshToken.IsRevoked.Should().BeTrue();
    }
}
