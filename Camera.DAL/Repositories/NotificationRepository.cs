using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Models;

namespace Camera.DAL.Repositories;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(ServerContext context) : base(context)
    {
    }
}