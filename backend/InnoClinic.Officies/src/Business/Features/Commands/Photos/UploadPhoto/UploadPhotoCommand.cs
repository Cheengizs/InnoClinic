using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Commands.Photos.UploadPhoto;

public record UploadPhotoCommand(Stream Stream, string ContentType) : IRequest<Result<Guid>>;
