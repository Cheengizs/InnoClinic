namespace DataAccess.Dto;

public class CreateOfficeDto
{
    public string City { get; set; }
    public string Street { get; set; } 
    public string HouseNumber { get; set; }
    public string OfficeNumber { get; set; } 
    public Guid? PhotoId { get; set; } 
    public string RegistryPhoneNumber { get; set; }
}
