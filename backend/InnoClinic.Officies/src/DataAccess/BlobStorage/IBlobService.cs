namespace DataAccess.BlobStorage;

public interface IBlobService
{
    Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken ct = default);
    Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken ct = default);
    Task DeleteAsync(Guid fileId, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid fileId, CancellationToken ct = default);
}
