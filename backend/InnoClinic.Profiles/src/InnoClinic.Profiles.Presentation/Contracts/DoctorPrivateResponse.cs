using InnoClinic.Profiles.Domain.Shared;

namespace InnoClinic.Profiles.Presentation.Contracts;

public class DoctorPrivateResponse
{
    public Guid Id { get; set; }
    public string? PhotoUrl { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public int CareerStartYear { get; set; }
    public DoctorStatus Status { get; set; }
}
