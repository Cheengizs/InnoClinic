namespace DataAccess.UsersService;

public interface IUsersService
{
    Task<List<string>> GetAllAdminsEmails(CancellationToken ct = default);
}
