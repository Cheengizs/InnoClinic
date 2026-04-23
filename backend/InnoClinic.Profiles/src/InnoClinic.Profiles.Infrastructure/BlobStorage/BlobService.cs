using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using InnoClinic.Profiles.Application.BlobStorage;
using Microsoft.Extensions.Options;

namespace InnoClinic.Profiles.Infrastructure.BlobStorage;

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
            new BlobHttpHeaders { ContentType = contentType },
            cancellationToken: ct);

        return fileId;
    }

    public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid fileId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsAsync(Guid fileId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
