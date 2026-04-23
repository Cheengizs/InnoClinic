using InnoClinic.Profiles.Domain.Shared;

namespace InnoClinic.Profiles.Application.ViewModels;

public class DoctorGet
{
    public Guid Id { get; set; }
    public string? PhotoUrl { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid AccountId { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public int CareerStartYear { get; set; }
    public DoctorStatus Status { get; set; }
}
