namespace DataAccess.Models;

public class Office
{
    public Guid Id { get; private set; }
    public string City { get; private set; }
    public string Street { get; private set; }
    public string HouseNumber { get; private set; }
    public string OfficeNumber { get; private set; }
    public Guid? PhotoId { get; private set; }
    public string RegistryPhoneNumber { get; private set; }
    public bool IsActive { get; private set; }

    public OfficePhoto Photo { get; private set; }

    public Office(string city, string street, string houseNumber, string officeNumber, string registryPhoneNumber,
        Guid? photoId = null, bool isActive = true)
    {
        Id = Guid.CreateVersion7();
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        OfficeNumber = officeNumber;
        PhotoId = photoId;
        RegistryPhoneNumber = registryPhoneNumber;
        IsActive = isActive;
    }

    private Office()
    {
    }

    public void SetActiveProperty(bool isActive)
    {
        IsActive = isActive;
    }

    public void ChangePhotoId(Guid? photoId)
    {
        PhotoId = photoId;
    }

    public void ChangeAddress(string city, string street, string houseNumber)
    {
        City = city;
        Street = street;
        HouseNumber = houseNumber;
    }

    public void ChangeOfficeNumber(string officeNumber)
    {
        OfficeNumber = officeNumber;
    }
    
    public void ChangeRegistryPhoneNumber(string registryPhoneNumber)
    {
        RegistryPhoneNumber = registryPhoneNumber;
    }
}
