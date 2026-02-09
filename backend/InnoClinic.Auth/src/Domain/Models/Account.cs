using Domain.Shared;

namespace Domain.Models;

public class Account
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string? PhoneNumber { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public AccountRole Role { get; private set; }
    public bool IsActive { get; private set; }
    
    private readonly List<RefreshToken> _refreshTokens = new();

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private Account() { }

    public Account(string email, string passwordHash, AccountRole role, string? phoneNumber = null)
    {
        Id = Guid.CreateVersion7();
        Email = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        IsEmailVerified = false;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Role = role;
    }

    public void VerifyEmail()
    {
        IsEmailVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        foreach (var t in _refreshTokens.Where(x => x.IsActive))
        {
            t.Revoke("Password changed");
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddRefreshToken(string tokenHash, int daysToExpire = 30)
    {
        var retentionThreshold = DateTime.UtcNow.AddYears(-1);
        _refreshTokens.RemoveAll(x => (x.IsExpired || x.RevokedAt != null) && x.CreatedAt < retentionThreshold);
        
        var refreshToken = new RefreshToken(this.Id, tokenHash, DateTime.UtcNow.AddDays(daysToExpire));
        
        _refreshTokens.Add(refreshToken);
    }

    public bool ValidateRefreshToken(string tokenHash)
    {
        if (!IsActive) return false;

        var rt = _refreshTokens.SingleOrDefault(x => x.TokenHash == tokenHash);
        return rt != null && rt.IsActive;
    }

    public void ChangeRole(AccountRole role)
    { 
        Role = role;
    }
    
    public void Deactivate()
    {
        IsActive = false;
        RevokeAllRefreshTokens("Account deactivated");
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void RevokeRefreshToken(string tokenHash, string reason)
    {
        var rt = _refreshTokens.SingleOrDefault(x => x.TokenHash == tokenHash);
        rt?.Revoke(reason);
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    private void RevokeAllRefreshTokens(string reason)
    {
        foreach (var t in _refreshTokens.Where(x => x.IsActive))
        {
            t.Revoke(reason);
        }
    }
}