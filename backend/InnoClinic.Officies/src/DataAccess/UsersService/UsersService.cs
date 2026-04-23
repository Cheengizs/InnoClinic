using MassTransit;
using Shared.Contracts;

namespace DataAccess.UsersService;

public class UsersService : IUsersService 
{
    private readonly IRequestClient<GetAdminEmailsRequest> _client;
    public UsersService(IRequestClient<GetAdminEmailsRequest> client) => _client = client;

    public async Task<List<string>> GetAllAdminsEmails(CancellationToken ct) 
    {
        var response = await _client.GetResponse<GetAdminEmailsResponse>(new(), ct);
        return response.Message.Emails.ToList();
    }
}
