using Camera.DAL.Interfaces.Repositories;

namespace Camera.DAL.Repositories;

public class CameraRepository : Repository<Joint.Data.Models.Camera>, ICameraRepository
{
    public CameraRepository(ServerContext context) : base(context)
    {
    }
}