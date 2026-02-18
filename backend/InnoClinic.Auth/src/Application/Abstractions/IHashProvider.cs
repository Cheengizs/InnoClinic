namespace Application.Abstractions;

public interface IHashProvider
{
    string GenerateHash(string rawData);    
}
