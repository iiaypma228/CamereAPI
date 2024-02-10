using System.Linq.Expressions;

namespace Camera.DAL.Interfaces;

//CRUD
public interface IRepository<T> where T : class
{
    void Save(); //commit
    IQueryable<T> Read();
    IQueryable<T> Read(Expression<Func<T, bool>> expressionWhere);
    IQueryable<T> ReadTracking(Expression<Func<T, bool>> expressionWhere);
    void Create(T item);
    void Update(T item);
    void Delete(T item);
}