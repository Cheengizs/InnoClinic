namespace DataAccess;

public class DbOptionClass
{
    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;

    public string ToConnectionString()
    {
        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            return $"mongodb://{Host}:{Port}";

        return $"mongodb://{Username}:{Password}@{Host}:{Port}/?authSource=admin";
    }
}
