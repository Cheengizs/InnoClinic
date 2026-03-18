using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories.Abstractions;
using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Offices.UpdateOffice;

public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand, Result>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;

    public UpdateOfficeCommandHandler(IOfficeRepository officeRepository, IMapper mapper)
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateOfficeCommand request, CancellationToken ct)
    {
        var officeFromRepo = await _officeRepository.GetOfficeByIdAsync(request.Id, ct);
        if (officeFromRepo == null)
        {
            return Result.Failure(ResultMessages.OfficeNotFound, ErrorType.NotFound);
        }
        
        var office = _mapper.Map<Office>(request);
        await _officeRepository.UpdateOfficeAsync(office, ct);
        
        return Result.Success();
    }
}
