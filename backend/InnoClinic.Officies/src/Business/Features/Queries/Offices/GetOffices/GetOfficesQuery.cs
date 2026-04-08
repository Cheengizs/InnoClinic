using Business.Contracts.Office;
using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Queries.Offices.GetOffices;

public record GetOfficesQuery(
    int PageNumber,
    int PageCount) : IRequest<Result<IEnumerable<OfficeGet>>>;
