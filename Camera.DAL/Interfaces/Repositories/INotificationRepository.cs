using Joint.Data.Models;

namespace Camera.DAL.Interfaces.Repositories;

public interface INotificationRepository : IRepository<Notification>, IDisposable
{
    List<Notification> GetNotificationsByCamera(int cameraId);
}
