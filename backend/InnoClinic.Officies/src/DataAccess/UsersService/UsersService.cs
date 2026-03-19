using System.Net.Http.Json;
using DataAccess.Options;
using Microsoft.Extensions.Options;

namespace DataAccess.UsersService;

public class UsersService : IUsersService
{
    private readonly HttpClient httpClient;
    private readonly UsersServiceOptions _options;
    
    public UsersService(HttpClient httpClient, IOptions<UsersServiceOptions> options)
    {
        this.httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<List<string>> GetAllAdminsEmails(CancellationToken ct = default)
    {
        var response = await httpClient.GetAsync(_options.Address, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        return await response.Content.ReadFromJsonAsync<List<string>>(cancellationToken: ct) ?? [];
    }
}
