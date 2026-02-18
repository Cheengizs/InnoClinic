using Domain.Models;

namespace Application.Abstractions;

public interface IJwtProvider
{
    string GenerateJwtToken(Account account);
    string GenerateRefreshToken();
}
