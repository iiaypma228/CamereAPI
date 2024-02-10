using System.Linq.Expressions;

namespace Camera.BLL.Interfaces;

public interface ICRUDService<T> : IDisposable where T : class
{
    void Save(T item);
    void Save(IList<T> items);
    IList<T> Read();
    IList<T> Read(Expression<Func<T,bool>> where);
    T Read(object id);

    void Delete(T item);
    void Delete(IList<T> items);
}