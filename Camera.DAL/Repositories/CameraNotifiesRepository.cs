using Camera.DAL.Interfaces.Repositories;

namespace Camera.DAL.Repositories;

public class CameraNotifiesRepository : Repository<Joint.Data.Models.CameraNotifies>, ICameraNotifiesRepository
{
    public CameraNotifiesRepository(ServerContext context) : base(context)
    {
    }
}