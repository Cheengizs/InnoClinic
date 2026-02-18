using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions;
using Application.Dto.Options;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly IOptions<JwtOptions> _configuration;

    public JwtProvider(IOptions<JwtOptions> configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(Account account)
    {
        var expTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration.Value.MinutesExp));
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, account.Email),
            new(ClaimTypes.Role, account.Role.ToString()),  
        ];

        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.SecretKey!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken securityToken = new(
            expires: expTime,
            signingCredentials: creds,
            audience: _configuration.Value.Audience,
            issuer: _configuration.Value.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
