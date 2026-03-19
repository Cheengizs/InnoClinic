namespace Business.Contracts.Photo;

public record PhotoDownloadResponse(Stream Stream, string ContentType);
