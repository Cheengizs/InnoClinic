using Domain.Shared;
using MediatR;

namespace Application.Features.Accounts.Queries.GetAdminsEmails;

public record GetAdminEmailsQuery() : IRequest<Result<List<string>>>;
