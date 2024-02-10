namespace Camera.BLL.Interfaces;

public interface ITwilioService : INotificationService
{
    Task SendMessage(string body, string to);
}