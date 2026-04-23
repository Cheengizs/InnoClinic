using InnoClinic.Profiles.Domain.Shared;

namespace InnoClinic.Profiles.Domain.Models;

public class Doctor
{
    public Guid Id { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? MiddleName { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public Guid AccountId { get; private set; }
    public Guid SpecializationId { get; private set; }
    public Guid OfficeId { get; private set; }
    public int CareerStartYear { get; private set; }
    public DoctorStatus Status { get; private set; }

    private Doctor() { }

    private Doctor(
        Guid id,
        string firstName,
        string lastName,
        string? middleName,
        DateOnly dateOfBirth,
        string email,
        Guid accountId,
        Guid specializationId,
        Guid officeId,
        int careerStartYear,
        DoctorStatus status,
        string? photoUrl)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        AccountId = accountId;
        Email = email;
        SpecializationId = specializationId;
        OfficeId = officeId;
        CareerStartYear = careerStartYear;
        Status = status;
        PhotoUrl = photoUrl;
    }

    public static Doctor Create(
        string firstName,
        string lastName,
        string? middleName,
        DateOnly dateOfBirth,
        string email,
        Guid accountId,
        Guid specializationId,
        Guid officeId,
        int careerStartYear,
        string? photoUrl = null)
    {
        return new Doctor(
            id: Guid.NewGuid(),
            firstName: firstName,
            lastName: lastName,
            middleName: middleName,
            dateOfBirth: dateOfBirth,
            accountId: accountId,
            email: email,
            specializationId: specializationId,
            officeId: officeId,
            careerStartYear: careerStartYear,
            status: DoctorStatus.AtWork,
            photoUrl: photoUrl);
    }

    public void UpdateProfile(
        string firstName, 
        string lastName, 
        string? middleName, 
        DateOnly dateOfBirth, 
        Guid specializationId, 
        Guid officeId, 
        int careerStartYear)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        SpecializationId = specializationId;
        OfficeId = officeId;
        CareerStartYear = careerStartYear;
    }

    public void ChangeStatus(DoctorStatus newStatus)
    {
        if (Status == newStatus)
            return;

        Status = newStatus;
    }

    public void UpdatePhoto(string photoUrl)
    {
        PhotoUrl = photoUrl;
    }

    public void RemovePhoto()
    {
        PhotoUrl = null;
    }
}
