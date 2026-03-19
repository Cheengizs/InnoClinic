using Business.Contracts.Photo;
using MediatR;
using Shared.Results;

namespace Business.Features.Queries.Photos.DownloadPhoto;

public record DownloadPhotoCommand(Guid Id) : IRequest<Result<PhotoDownloadResponse>>;

