using InnoClinic.Profiles.Application.Repositories;
using InnoClinic.Profiles.Domain.Models;
using InnoClinic.Profiles.Infrastructure.DbContexts;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfilesDbContext _dbContext;

    public DoctorRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Doctor> CreateDoctorAsync(Doctor doctor, CancellationToken ct = default)
    {
        _dbContext.Doctors.Add(doctor);
        await _dbContext.SaveChangesAsync(ct);

        return doctor;
    }

    public async Task<Doctor?> GetDoctorByEmailAsync(string email, CancellationToken token = default)
    {
        return _dbContext.Doctors.FirstOrDefault(d => d.Email == email);
    }

    public async Task<Doctor?> GetDoctorByIdAsync(Guid id, CancellationToken token = default)
    {
        return _dbContext.Doctors.FirstOrDefault(d => d.Id == id);
    }
}
