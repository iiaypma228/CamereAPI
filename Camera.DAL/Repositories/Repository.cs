using System.Linq.Expressions;
using Camera.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Camera.DAL.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{

    public ServerContext context { get; private set; }
    public Repository(ServerContext context)
    {
        this.context = context;
    }        
    public void Save()
    {
        this.context.SaveChanges();
    }
    #region -- CRUD --

    public virtual IQueryable<T> Read()
    {            
        return this.context.Set<T>().AsNoTracking();
    }

    public virtual IQueryable<T> Read(Expression<Func<T, bool>> expressionWhere)
    {
        return this.context.Set<T>().AsNoTracking().Where(expressionWhere);
    }
    public virtual IQueryable<T> ReadTracking(Expression<Func<T, bool>> expressionWhere)
    {
        return this.context.Set<T>().Where(expressionWhere);
    }

    public virtual void Create(T item)
    {
        this.context.ChangeTracker.AutoDetectChangesEnabled = false;
        this.context.Set<T>().Add(item);
    }        

    public virtual void Update(T item)
    {
        this.context.ChangeTracker.AutoDetectChangesEnabled = false;
        this.context.Entry(item).State = EntityState.Modified;
    }

    public virtual void Delete(T item)
    {
        this.context.ChangeTracker.AutoDetectChangesEnabled = false;
        this.context.Entry(item).State = EntityState.Deleted;
    }
    #endregion

    //public virtual void Reload(T item)
    //{
    //    this.context.Entry(item).Reload();
    //}

    public void Dispose()
    {
        //this.context.Set<T>().Dispose();
    }
}