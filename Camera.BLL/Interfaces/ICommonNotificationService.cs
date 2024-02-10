using Joint.Data.Models;

namespace Camera.BLL.Interfaces;

public interface ICommonNotificationService : ICRUDService<Notification>
{
    void Notify(NotifyToSend notifyToSend);
}