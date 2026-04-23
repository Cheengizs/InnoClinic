using InnoClinic.Profiles.Domain.Models;

namespace InnoClinic.Profiles.Application.Repositories;

public interface IDoctorRepository
{
    Task<Doctor> CreateDoctorAsync(Doctor doctor, CancellationToken token = default);
    Task<Doctor?> GetDoctorByEmailAsync(string email, CancellationToken token = default);
    Task<Doctor?> GetDoctorByIdAsync(Guid id, CancellationToken token = default);
}
