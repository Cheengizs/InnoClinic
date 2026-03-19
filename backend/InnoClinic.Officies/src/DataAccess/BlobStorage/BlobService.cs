using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace DataAccess.BlobStorage;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobStorageOptions _blobStorageOptions;
    
    public BlobService(BlobServiceClient blobServiceClient, IOptions<BlobStorageOptions> blobStorageOptions)
    {
        _blobServiceClient = blobServiceClient;
        _blobStorageOptions = blobStorageOptions.Value;
    }

    public async Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken ct = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobStorageOptions.ContainerName);
        
        var fileId = Guid.NewGuid();
        var blobClient = containerClient.GetBlobClient(fileId.ToString());
        
        await blobClient.UploadAsync(stream,
            new BlobHttpHeaders{ContentType = contentType},
            cancellationToken : ct);

        return fileId;
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken ct = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobStorageOptions.ContainerName);
        var blobClient = containerClient.GetBlobClient(fileId.ToString());
        var response = await blobClient.DownloadContentAsync(cancellationToken: ct);
        
        return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
    }

    public async Task DeleteAsync(Guid fileId, CancellationToken ct = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobStorageOptions.ContainerName);
        var blobClient = containerClient.GetBlobClient(fileId.ToString());
        await blobClient.DeleteIfExistsAsync(cancellationToken: ct);
    }

    public async Task<bool> ExistsAsync(Guid fileId, CancellationToken ct = default)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_blobStorageOptions.ContainerName);
        var blobClient = blobContainer.GetBlobClient(fileId.ToString());
        return await blobClient.ExistsAsync();
    }
}
