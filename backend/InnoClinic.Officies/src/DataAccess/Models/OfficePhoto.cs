namespace DataAccess.Models;

public class OfficePhoto
{
    public Guid Id { get; private set; }
    public string Url { get; private set; }
    public Office Office { get; private set; }
    
    public OfficePhoto(string url)
    {
        Id = Guid.CreateVersion7();
        Url = url;
    }
    
    private OfficePhoto() { }
}
