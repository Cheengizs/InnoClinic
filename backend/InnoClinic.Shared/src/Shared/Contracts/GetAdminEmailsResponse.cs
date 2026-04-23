namespace Shared.Contracts;

public record GetAdminEmailsResponse(IEnumerable<string> Emails);
