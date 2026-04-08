using InnoClinic.Officies.Shared.Results;
using MediatR;

namespace Business.Features.Commands.Photos.DeletePhoto;

public record DeletePhotoCommand(Guid PhotoId) : IRequest<Result>;
