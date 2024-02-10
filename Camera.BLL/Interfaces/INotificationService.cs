using Joint.Data.Models;

namespace Camera.BLL.Interfaces;

public interface INotificationService
{
    void Notify(NotifyToSend notifyToSend);
}