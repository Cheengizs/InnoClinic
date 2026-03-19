using Business.Contracts.Office;
using MediatR;
using Shared.Results;

namespace Business.Features.Queries.Offices.GetOffices;

public record GetOfficesQuery(
    int PageNumber,
    int PageCount) : IRequest<Result<IEnumerable<OfficeGet>>>;
