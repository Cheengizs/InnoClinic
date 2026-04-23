namespace InnoClinic.Profiles.Infrastructure.MessageBroker;

public class RabbitMqOptions
{
    public string Host { get; set; } = string.Empty;
    public string VirtualHost { get; set; }  = string.Empty;
    public string Username { get; set; }  = string.Empty;
    public string Password { get; set; }  = string.Empty;
}
