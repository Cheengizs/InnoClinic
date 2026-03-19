using MediatR;
using Shared.Results;

namespace Business.Features.Commands.Photos.DeletePhoto;

public record DeletePhotoCommand(Guid PhotoId) : IRequest<Result>;

