using Business.Contracts.Photo;
using DataAccess.BlobStorage;
using MediatR;
using Shared.Results;

namespace Business.Features.Queries.Photos.DownloadPhoto;

public class DownloadPhotoCommandHandler : IRequestHandler<DownloadPhotoCommand, Result<PhotoDownloadResponse>>
{
    private readonly IBlobService _blobService;

    public DownloadPhotoCommandHandler(IBlobService blobService)
    {
        _blobService = blobService;
    }
    
    public async Task<Result<PhotoDownloadResponse>> Handle(DownloadPhotoCommand request, CancellationToken ct)
    {
        var isExists = await _blobService.ExistsAsync(request.Id);
        if (!isExists)
        {
            return Result<PhotoDownloadResponse>.Failure(ResultMessages.PhotoNotFound, ErrorType.NotFound);
        }

        var photo = await _blobService.DownloadAsync(request.Id, ct);
        var result = new PhotoDownloadResponse(photo.Stream, photo.ContentType);
        return Result<PhotoDownloadResponse>.Success(result);
    }
}
