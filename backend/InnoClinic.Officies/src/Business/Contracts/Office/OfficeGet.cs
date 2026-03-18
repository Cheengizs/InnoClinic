namespace Business.Contracts.Office;

public class OfficeGet
{
    public Guid Id { get; set; }
    public string City { get; set; }
    public string Street { get;  set; }
    public string HouseNumber { get; set; }
    public string OfficeNumber { get; set; }
    public string PhotoUri { get;  set; }
    public string RegistryPhoneNumber { get; set; }
    public bool IsActive { get; set; }
}   
