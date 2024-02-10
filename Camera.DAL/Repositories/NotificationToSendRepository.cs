using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Models;

namespace Camera.DAL.Repositories;

public class NotificationToSendRepository : Repository<NotifyToSend>, INotificationToSendRepository
{
    public NotificationToSendRepository(ServerContext context) : base(context)
    {
    }
}