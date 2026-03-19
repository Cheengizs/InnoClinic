using DataAccess.BlobStorage;
using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Photos.UploadPhoto;

public class UploadPhotoCommandHandler : IRequestHandler<UploadPhotoCommand, Result<Guid>>
{
    private readonly IBlobService _blobService;

    public UploadPhotoCommandHandler(IBlobService blobService)
    {
        _blobService = blobService;
    }

    public async Task<Result<Guid>> Handle(UploadPhotoCommand request, CancellationToken ct = default)
    {
        if (request.Stream.Length > 10 * 1024 * 1024 || request.Stream.Length == 0)
            return Result<Guid>.Failure(ResultMessages.PhotoIsTooLarge, ErrorType.Validation);

        var result = await _blobService.UploadAsync(request.Stream, request.ContentType, ct);
        return Result<Guid>.Success(result);
    }
}
