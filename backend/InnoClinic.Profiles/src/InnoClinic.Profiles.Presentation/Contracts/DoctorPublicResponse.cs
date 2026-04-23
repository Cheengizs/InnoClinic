namespace InnoClinic.Profiles.Presentation.Contracts;

public class DoctorPublicResponse
{
    
    public Guid Id { get; set; }
    public string? PhotoUrl { get; set; }
    public string FullName { get; set; } = string.Empty;
    public Guid OfficeId { get; set; }
    public int Experience { get; set; }
    public Guid SpecializationId { get; set; }
}
