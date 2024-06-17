using System.Linq.Expressions;
using System.Text;
using Camera.BLL.Interfaces;
using Camera.DAL;
using Camera.DAL.Interfaces.Repositories;
using Joint.Data.Models;

namespace Camera.BLL.Services;

public class UserService : Service, IUserService
{
    private IUserRepository _repository;
    
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public void Save(User item)
    {
        this.Save(new List<User>() {item});   
    }

    public void Save(IList<User> items)
    {
        foreach (var item in items)
        {
            if (item.Id == 0 || this.Read(item.Id) == null)
            {
                if (this.ReadByEmail(item.Email) != null)
                {
                    throw new Exception("Користувач вже існує");
                }
                var password = item.Password;
                if (string.IsNullOrEmpty(password))
                    password = item.Email;
                var bytes = new UTF8Encoding().GetBytes(password);
                byte[] hashBytes;
                using (var algorithm = new System.Security.Cryptography.SHA256Managed())
                {
                    hashBytes = algorithm.ComputeHash(bytes);
                }
                item.Password = Convert.ToBase64String(hashBytes);
                this._repository.Create(item);
                
            }
            else
            {
                if (this.ReadByEmail(item.Email) != null)
                {
                    throw new Exception("Користувач вже існує з цією почтою вже існує");
                }
                var oldUser = Read(item.Id);
                item.Password = oldUser.Password;
                this._repository.Update(item);
            }
        }
        
        this._repository.Save();
    }

    public IList<User> Read()
    {
        return this._repository.Read(i => true).ToList();
    }

    public IList<User> Read(Expression<Func<User, bool>> where)
    {
        return this._repository.Read(where).ToList();
    }

    public User Read(object id)
    {
        return this._repository.Read(i => i.Id == (int)id).FirstOrDefault();
    }

    public void Delete(User item)
    {
        this.Delete(new List<User>() { item });
    }

    public void Delete(IList<User> items)
    {
        foreach (var item in items)
        {
            var exsist = this.Read(item.Id);

            if (exsist != null)
            {
                this._repository.Delete(exsist);
            }
            
        }
        this._repository.Save();
    }

    public User ReadByEmail(string email)
    {
        return this._repository.Read(i => i.Email == email).FirstOrDefault();
    }

    public User IsUserAuth(User user)
    {
        // находим пользователя
        var userExsist = this.ReadByEmail(user.Email);
        // проверка авторизации
        if (userExsist != null)
        {
            var bytes = new UTF8Encoding().GetBytes(user.Password);
            byte[] hashBytes;
            using (var algorithm = new System.Security.Cryptography.SHA256Managed())
            {
                hashBytes = algorithm.ComputeHash(bytes);
            }
            string passwordHash = Convert.ToBase64String(hashBytes);

            if (userExsist.Password != passwordHash)
                userExsist = null;
        }

        return userExsist;
    }

    public void Change(User user)
    {
        var oldUser = Read(user.Id);
        user.Password = oldUser.Password;
        this._repository.Update(user);
        this._repository.Save();
    }

    public void Dispose()
    {
        _repository.Dispose();
    }
}