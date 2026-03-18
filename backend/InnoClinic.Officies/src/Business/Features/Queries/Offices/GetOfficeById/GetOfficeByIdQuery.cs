using Business.Contracts.Office;
using MediatR;
using Shared.Results;

namespace Business.Features.Queries.Offices.GetOfficeById;

public record GetOfficeByIdQuery(Guid Id) : IRequest<Result<OfficeGet>>;
