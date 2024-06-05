using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Constants;
using Joint.Data.Models;

namespace Camera.DAL.Repositories;

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(ServerContext context) : base(context)
    {
    }

    public List<Notification> GetNotificationsByCamera(int cameraId)
    {
        return this.context.CameraNotifies
            .GroupJoin(this.context.Cameras, cn => cn.CameraId, c => c.Id, (cn, c) => new { cn, c })
            .SelectMany(o => o.c.DefaultIfEmpty(), (cn, c) => new { cn.cn, c })
            .GroupJoin(this.context.Notifications, cn => cn.cn.NotificationId, n => n.Id,
                (cn, n) => new { cn.cn, cn.c, n })
            .SelectMany(o => o.n.DefaultIfEmpty(), (cn, n) => new { cn.cn, cn.c, n })
            .GroupJoin(this.context.Users, cn => cn.n.UserId, u => u.Id, (cn, u) => new { cn.cn, cn.c, cn.n, u })
            .SelectMany(o => o.u.DefaultIfEmpty(), (cn, u) => new { cn.cn, cn.c, cn.n, u })
            .Where(i => i.c.Id == cameraId)
            .Select(i => new Notification
            {
                Id = i.n.Id,
                UserId = i.u.Id,
                User = i.u,
                Name = i.n.Name,
                NotificationType = i.n.NotificationType,
                Data = i.n.Data
            }).ToList();

    }
}