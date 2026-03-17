using Business.Contracts.Office;
using MediatR;
using Shared.Results;

namespace Business.Features.Queries.Officies.GetOffices;

public record GetOfficesCommand(int PageNumber,
    int PageCount) : IRequest<Result<IEnumerable<OfficeGet>>>;
