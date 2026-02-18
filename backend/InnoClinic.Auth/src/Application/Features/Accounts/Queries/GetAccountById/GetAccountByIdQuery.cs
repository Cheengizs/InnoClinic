using Application.Dto.Account;
using Domain.Shared;
using MediatR;

namespace Application.Features.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IRequest<Result<AccountResponse>>;
