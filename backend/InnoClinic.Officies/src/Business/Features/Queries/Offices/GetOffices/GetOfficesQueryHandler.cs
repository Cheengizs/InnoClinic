using AutoMapper;
using Business.Contracts.Office;
using DataAccess.Repositories.Abstractions;
using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Queries.Offices.GetOffices;

public class GetOfficesQueryHandler : IRequestHandler<GetOfficesQuery, Result<IEnumerable<OfficeGet>>>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;

    public GetOfficesQueryHandler(IOfficeRepository officeRepository, IMapper mapper)
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<OfficeGet>>> Handle(GetOfficesQuery request, CancellationToken ct)
    {
        var officies = await _officeRepository.GetAllOfficesAsync(request.PageNumber, request.PageCount, ct);
        var result = _mapper.Map<IEnumerable<OfficeGet>>(officies);
        return Result<IEnumerable<OfficeGet>>.Success(result);
    }
}
