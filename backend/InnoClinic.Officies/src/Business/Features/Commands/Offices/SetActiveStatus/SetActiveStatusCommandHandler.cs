using DataAccess.Repositories.Abstractions;
using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Offices.SetActiveStatus;

public class SetActiveStatusCommandHandler : IRequestHandler<SetActiveStatusCommand, Result>
{
    private readonly IOfficeRepository _officeRepository;

    public SetActiveStatusCommandHandler(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<Result> Handle(SetActiveStatusCommand request, CancellationToken ct)
    {
        var office = await _officeRepository.GetOfficeByIdAsync(request.Id, ct);
        if (office == null)
        {
            return Result.Failure(ResultMessages.OfficeNotFound, ErrorType.NotFound);
        }

        var res = await _officeRepository.SetActiveStatusAsync(office, request.IsActive, ct);
        if (res != request.IsActive)
        {
            return Result.Failure(ResultMessages.UnexpectedResult);
        }

        return Result.Success();
    }
}
