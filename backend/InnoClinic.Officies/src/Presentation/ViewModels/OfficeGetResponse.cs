namespace Presentation.ViewModels;

public class OfficeGetResponse
{
    public Guid Id { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string HouseNumber { get; init; }
    public string OfficeNumber { get; init; }
    public string PhotoUri { get; init; }
    public string RegistryPhoneNumber { get; init; }
    public bool IsActive { get; init; }
}
