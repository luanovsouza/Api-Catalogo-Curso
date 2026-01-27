using System.Linq.Expressions;

namespace ApiCatalogo.Repositories.Interfaces;

public interface IRepositoryGeneric<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    
    Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate);
    
    T Create(T entity); // Nao precisa ser assincrono pois o UoF a faz o saveChangesAsync, e tbm nao acessam banco de dados
    
    T Update(T entity);// Nao precisa ser assincrono pois o UoF a faz o saveChangesAsync, e tbm nao acessam banco de dados
    
    T Delete(T entity);// Nao precisa ser assincrono pois o UoF a faz o saveChangesAsync, e tbm nao acessam banco de dados
}