namespace InnoClinic.Profiles.Infrastructure.MessageBroker;

public interface IUsersService
{
    Task SendMessageAsync<T>(T message);
}
