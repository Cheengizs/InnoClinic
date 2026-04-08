using DataAccess.BlobStorage;
using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Commands.Photos.DeletePhoto;

public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, Result>
{
    private readonly IBlobService _blobService;

    public DeletePhotoCommandHandler(IBlobService blobService)
    {
        _blobService = blobService;
    }

    public async Task<Result> Handle(DeletePhotoCommand request, CancellationToken ct = default)
    {
        var isExists = await _blobService.ExistsAsync(request.PhotoId, ct);
        if (!isExists)
        {
            return Result.Failure(ResultMessages.PhotoNotFound, ErrorType.NotFound);
        }

        await _blobService.DeleteAsync(request.PhotoId, ct);
        return Result.Success();
    }
}
