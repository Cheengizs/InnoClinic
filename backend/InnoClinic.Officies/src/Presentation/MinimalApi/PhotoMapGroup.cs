using Business.Features.Commands.Photos.DeletePhoto;
using Business.Features.Commands.Photos.UploadPhoto;
using Business.Features.Queries.Photos.DownloadPhoto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;

namespace Presentation.MinimalApi;

public static class PhotoMapGroup
{
    public static RouteGroupBuilder MapPhotos(this RouteGroupBuilder group)
    {
        group.MapGet("{id:guid}",
                async (IMediator mediator, [FromRoute] Guid id, CancellationToken ct) =>
                {
                    var request = new DownloadPhotoCommand(id);
                    var result = await mediator.Send(request, ct);
                    if (!result.IsSuccess)
                    {
                        return result.ToProblemDetails();
                    }
                    
                    var resultValue = result.Value;
                    return Results.File(resultValue!.Stream, resultValue.ContentType);
                })
            .WithName("GetPhotoById");

        group.MapPost("/upload",
                async (IMediator mediator, IFormFile file, CancellationToken ct) =>
                {
                    await using var stream = file.OpenReadStream();
                    var request = new UploadPhotoCommand(stream, file.ContentType);

                    var result = await mediator.Send(request, ct);

                    if (!result.IsSuccess)
                    {
                        return result.ToProblemDetails();
                    }

                    var resultValue = result.Value;
                    return Results.CreatedAtRoute("GetPhotoById", new { id = resultValue }, resultValue);
                })
            .DisableAntiforgery();

        group.MapDelete("{id:guid}", async ([FromRoute] Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var request = new DeletePhotoCommand(id);
            var result = await mediator.Send(request, ct);
            if (!result.IsSuccess)
            {
                return result.ToProblemDetails();
            }

            return Results.NoContent();
        });

        return group;
    }
}
