using DataAccess.BlobStorage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace Presentation.MinimalApi;

public static class PhotoMapGroup
{
    public static RouteGroupBuilder MapPhotos(this RouteGroupBuilder group)
    {
        group.MapGet("{id:guid}", async (IMediator mediator, IBlobService blobService, [FromRoute] Guid id, CancellationToken ct) =>
        {
            try
            {
                var fileResponse = await blobService.DownloadAsync(id, ct);
                return Results.File(fileResponse.Stream, fileResponse.ContentType);
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                return Results.NotFound();
            }
            
        })
        .WithName("GetPhotoById");
        
        group.MapPost("/upload", async (IFormFile file, IBlobService blobService, CancellationToken ct) =>
            {
                if (file.Length == 0) return Results.BadRequest("File is empty");

                using var stream = file.OpenReadStream();
                var fileId = await blobService.UploadAsync(stream, file.ContentType, ct);

                return Results.CreatedAtRoute("GetPhotoById", new { id = fileId },  fileId);
            })
            .DisableAntiforgery();

        group.MapDelete("{id:guid}", async (Guid id, IBlobService blobService, CancellationToken ct) =>
        {
            var isExists = await blobService.ExistsAsync(id, ct);
            if (!isExists) return Results.NotFound();
            
            await blobService.DeleteAsync(id, ct);
            return Results.NoContent();
        });
        
        return group;
    }
}
