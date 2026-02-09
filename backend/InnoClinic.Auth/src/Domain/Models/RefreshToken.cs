namespace Domain.Models;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public string TokenHash { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }
    public string? RevokedReason { get; private set; }
    public Guid AccountId { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken()
    {
    }

    public RefreshToken(Guid accountId, string tokenHash, DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public void Revoke(string reason)
    {
        RevokedAt = DateTime.UtcNow;
        RevokedReason = reason;
    }
}