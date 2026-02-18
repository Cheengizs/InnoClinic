using Application.Abstractions;

namespace Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string passwor, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(passwor, hashedPassword);
    }
}
