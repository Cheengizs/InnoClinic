using AutoMapper;
using DataAccess.Repositories.Abstractions;
using InnoClinic.Officies.Shared.Results;
using MediatR;

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
        var office = await _officeRepository.GetOfficeByIdAsync(request.Id, ct);
        if (office == null)
        {
            return Result.Failure(ResultMessages.OfficeNotFound, ErrorType.NotFound);
        }

        _mapper.Map(request, office);
        await _officeRepository.UpdateOfficeAsync(office, ct);

        return Result.Success();
    }
}
