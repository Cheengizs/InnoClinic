using Application.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security;

public class HashProvider : IHashProvider
{
    public string GenerateHash(string rawData)
    {
        var bytes = Encoding.UTF8.GetBytes(rawData);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
