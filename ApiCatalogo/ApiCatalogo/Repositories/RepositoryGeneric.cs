using System.Linq.Expressions;
using ApiCatalogo.Context;
using ApiCatalogo.Repositories.Interfaces;

namespace ApiCatalogo.Repositories;

public class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : class
{
    protected readonly AppDbContext _context;

    public RepositoryGeneric(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        //Esse "set" ele é feito para acessar uma tabela ou uma coleçao em um banco de dados
        return _context.Set<T>().ToList();
    }

    public T? GetById(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
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