using Domain.Shared;

namespace Application.Dto.Account;

public record AccountResponse(
    Guid Id,
    string Email,
    AccountRole Role,
    string? PhoneNumber,
    DateTime CreatedAt
);
