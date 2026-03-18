using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories.Abstractions;
using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Officies.CreateOffice;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, Result<Guid>>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;

    public CreateOfficeCommandHandler(IOfficeRepository officeRepository, IMapper mapper)
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
    }


    public async Task<Result<Guid>> Handle(CreateOfficeCommand request, CancellationToken ct)
    {
        var office = _mapper.Map<Office>(request);
        
        await _officeRepository.CreateOfficeAsync(office, ct);
        
        return Result<Guid>.Success(office.Id);
    }
}
