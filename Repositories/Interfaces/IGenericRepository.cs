using System.Linq.Expressions;

namespace TP.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    // Basic CRUD
    IEnumerable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();
    
    T? GetById(object id);
    Task<T?> GetByIdAsync(object id);
    
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    
    void Add(T entity);
    Task AddAsync(T entity);
    
    void AddRange(IEnumerable<T> entities);
    Task AddRangeAsync(IEnumerable<T> entities);
    
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    
    int SaveChanges();
    Task<int> SaveChangesAsync();
}   