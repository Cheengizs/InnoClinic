namespace Presentation.ViewModels;

public class OfficeUpdateRequest
{
    public Guid Id { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
    public string HouseNumber { get; init; }
    public string OfficeNumber { get; init; }
    public Guid? PhotoId { get; init; }
    public string RegistryPhoneNumber { get; init; }
}
