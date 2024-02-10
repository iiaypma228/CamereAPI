using Joint.Data.Models;

namespace Camera.BLL.Interfaces;

public interface IUserService : ICRUDService<User>
{
    User ReadByEmail(string email);
    User IsUserAuth(User user);
}