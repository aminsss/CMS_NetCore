using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CMS_NetCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS_NetCore.DataLayer;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private AppDbContext RepositoryContext { get; }
    private readonly DbSet<T> _dbSet;


    protected RepositoryBase(AppDbContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
        _dbSet = repositoryContext.Set<T>();
    }

    public IQueryable<T> FindAll()
    {
        return _dbSet;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await RepositoryContext.SaveChangesAsync();
    }
}