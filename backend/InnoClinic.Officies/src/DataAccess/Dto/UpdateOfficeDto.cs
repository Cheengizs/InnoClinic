namespace DataAccess.Dto;

public class UpdateOfficeDto
{
    public Guid Id { get; private set; }
    public string City { get; private set; }
    public string Street { get; private set; }
    public string HouseNumber { get; private set; }
    public string OfficeNumber { get; private set; }
    public Guid? PhotoId { get; private set; }
    public string RegistryPhoneNumber { get; private set; }
    public bool IsActive { get; private set; }
}
