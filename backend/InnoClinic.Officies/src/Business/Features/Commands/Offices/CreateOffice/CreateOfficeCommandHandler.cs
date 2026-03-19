using AutoMapper;
using Business.Contracts.Office;
using Business.Features.Notifications;
using DataAccess.Models;
using DataAccess.Repositories.Abstractions;
using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Offices.CreateOffice;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand, Result<OfficeGet>>
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public CreateOfficeCommandHandler(IOfficeRepository officeRepository, IMapper mapper, IMediator mediator)
    {
        _officeRepository = officeRepository;
        _mapper = mapper;
        _mediator = mediator;
    }


    public async Task<Result<OfficeGet>> Handle(CreateOfficeCommand request, CancellationToken ct)
    {
        var office = _mapper.Map<Office>(request);

        office = await _officeRepository.CreateOfficeAsync(office, ct);
        var officeCreatedNotify = new OfficeCreatedNotification(office.Id, office.OfficeNumber, office.City);
        await _mediator.Publish(officeCreatedNotify, ct);
        var result = _mapper.Map<OfficeGet>(office);
        return Result<OfficeGet>.Success(result);
    }
}
