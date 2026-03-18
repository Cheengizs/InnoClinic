using DataAccess.Repositories.Abstractions;
using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Officies.DeleteOffice;

public class DeleteOfficeCommandHandler : IRequestHandler<DeleteOfficeCommand, Result>
{
    private readonly IOfficeRepository _officeRepository;

    public DeleteOfficeCommandHandler(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<Result> Handle(DeleteOfficeCommand request, CancellationToken ct)
    {
        var office = await _officeRepository.GetOfficeByIdAsync(request.Id, ct);

        if (office == null)
        {
            return Result.Failure(ResultMessages.OfficeNotFound, ErrorType.NotFound);
        }
        
        await _officeRepository.DeleteOfficeAsync(office, ct);
        return Result.Success();
    }
}
