using System.Linq.Expressions;
using APICatalogo.Context;

namespace APICatalogo.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().SingleOrDefault(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();

        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();

        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();

        return entity;
    }
}