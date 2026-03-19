using AutoMapper;
using Business.Contracts.Office;
using DataAccess.Repositories.Abstractions;
using MediatR;
using Shared.Results;

namespace Business.Features.Queries.Offices.GetOfficeById;

public class GetOfficeByIdQueryHandler : IRequestHandler<GetOfficeByIdQuery, Result<OfficeGet>>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;

    public GetOfficeByIdQueryHandler(IOfficeRepository officeRepository, IMapper mapper)
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
    }

    public async Task<Result<OfficeGet>> Handle(GetOfficeByIdQuery request, CancellationToken ct)
    {
        var office = await _officeRepository.GetOfficeByIdAsync(request.Id, ct);
        if (office == null)
        {
            return Result<OfficeGet>.Failure(ResultMessages.OfficeNotFound, ErrorType.NotFound);
        }

        var officeContract = _mapper.Map<OfficeGet>(office);

        return Result<OfficeGet>.Success(officeContract);
    }
}
