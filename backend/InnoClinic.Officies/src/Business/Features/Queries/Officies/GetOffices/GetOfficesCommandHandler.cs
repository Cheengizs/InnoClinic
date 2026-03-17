using AutoMapper;
using Business.Contracts.Office;
using DataAccess.Repositories.Abstractions;
using MediatR;
using Shared.Results;

namespace Business.Features.Queries.Officies.GetOffices;

public class GetOfficesCommandHandler : IRequestHandler<GetOfficesCommand, Result<IEnumerable<OfficeGet>>>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;

    public GetOfficesCommandHandler(IOfficeRepository officeRepository, IMapper mapper)
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<OfficeGet>>> Handle(GetOfficesCommand request, CancellationToken ct)
    {
        var officies = await _officeRepository.GetAllOfficesAsync(request.PageNumber, request.PageNumber, ct);
        var result = _mapper.Map<IEnumerable<OfficeGet>>(officies);
        return Result<IEnumerable<OfficeGet>>.Success(result);
    }
}
