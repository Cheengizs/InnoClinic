using Application.Features.Accounts.Queries.GetAccountById;
using AuthTests.Resources;
using Domain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AuthTests.Features.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandlerTests
{
    private readonly TestAuthDbContext _dbContext;

    public GetAccountByIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestAuthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TestAuthDbContext(options);
    }

    [Fact]
    public async Task Handle_AccountDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var query = new GetAccountByIdQuery(Guid.NewGuid());
        var handler = new GetAccountByIdQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_AccountExists_ReturnsSuccessWithData()
    {
        // Arrange
        var account = new Account(
            email: "query@test.com", 
            passwordHash: "hash", 
            phoneNumber: "999888777", 
            role: default
        );
        
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        var query = new GetAccountByIdQuery(account.Id);
        var handler = new GetAccountByIdQueryHandler(_dbContext);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be(account.Email);
        result.Value.Id.Should().Be(account.Id);
        result.Value.PhoneNumber.Should().Be(account.PhoneNumber);
    }
}
