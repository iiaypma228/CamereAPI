using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Models;

namespace Camera.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ServerContext context) : base(context)
    {
    }
}