using Business.Contracts.Office;
using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Queries.Offices.GetOfficeById;

public record GetOfficeByIdQuery(Guid Id) : IRequest<Result<OfficeGet>>;
