using Microsoft.Extensions.Options;

namespace Infrastructure.Options;

public class DbConnectionOption 
{
    public string? DbConnectionString { get; init; }
}
