using AutoMapper;
using Business.Contracts.Office;
using DataAccess.Models;
using DataAccess.Repositories.Abstractions;
using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Offices.CreateOffice;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, Result<OfficeGet>>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;

    public CreateOfficeCommandHandler(IOfficeRepository officeRepository, IMapper mapper)
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
    }


    public async Task<Result<OfficeGet>> Handle(CreateOfficeCommand request, CancellationToken ct)
    {
        var office = _mapper.Map<Office>(request);
        
        office = await _officeRepository.CreateOfficeAsync(office, ct);
        
        var result = _mapper.Map<OfficeGet>(office);
        return Result<OfficeGet>.Success(result);
    }
}
