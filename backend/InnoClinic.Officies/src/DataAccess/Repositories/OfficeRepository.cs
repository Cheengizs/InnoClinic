using DataAccess.DbContexts;
using DataAccess.Models;
using DataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class OfficeRepository : IOfficeRepository
{
    private readonly OfficesDbContext _dbContext;

    public OfficeRepository(OfficesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Office?> GetOfficeByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext
            .Offices
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Office>> GetAllOfficesAsync(int pageNumber, int pageCount, CancellationToken ct)
    {
        return await _dbContext.Offices
            .AsNoTracking()
            .OrderBy(o => o.Id)
            .Skip((pageNumber - 1) * pageCount)
            .Take(pageCount)
            .ToListAsync(ct);
    }

    public async Task<Office?> CreateOfficeAsync(Office office, CancellationToken ct)
    {
        _dbContext.Offices.Add(office);
        await _dbContext.SaveChangesAsync(ct);

        return office;
    }

    public async Task<Office?> UpdateOfficeAsync(Office office, CancellationToken ct)
    {
        _dbContext.Offices.Update(office);
        await _dbContext.SaveChangesAsync(ct);

        return office;
    }

    public async Task DeleteOfficeAsync(Office office, CancellationToken ct)
    {
        _dbContext.Offices.Remove(office);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<bool> SetActiveStatusAsync(Office office, bool newIsActive, CancellationToken ct)
    {
        office.SetActiveProperty(newIsActive);
        await _dbContext.SaveChangesAsync(ct);
        return office.IsActive;
    }
}
