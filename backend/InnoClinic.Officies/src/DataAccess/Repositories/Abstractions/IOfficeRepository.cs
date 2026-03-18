using DataAccess.Dto;
using DataAccess.Models;

namespace DataAccess.Repositories.Abstractions;

public interface IOfficeRepository
{
    Task<Office?> GetOfficeByIdAsync(Guid id, CancellationToken ct);
    Task<List<Office>> GetAllOfficesAsync(int pageNumber, int pageCount, CancellationToken ct);
    Task<Office?> CreateOfficeAsync(Office office, CancellationToken ct);
    Task<Office?> UpdateOfficeAsync(Office office, CancellationToken ct);
    Task DeleteOfficeAsync(Office office, CancellationToken ct);
    Task<bool> SetActiveStatusAsync(Office office, bool newIsActive, CancellationToken ct);
}
