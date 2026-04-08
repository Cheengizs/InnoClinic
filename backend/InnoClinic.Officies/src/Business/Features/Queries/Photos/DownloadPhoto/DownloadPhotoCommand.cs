using Business.Contracts.Photo;
using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Queries.Photos.DownloadPhoto;

public record DownloadPhotoCommand(Guid Id) : IRequest<Result<PhotoDownloadResponse>>;
