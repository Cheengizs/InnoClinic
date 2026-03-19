using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Photos.UploadPhoto;

public record UploadPhotoCommand(Stream Stream, string ContentType) : IRequest<Result<Guid>>;
