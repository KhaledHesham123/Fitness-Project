using NutritionService.Models;
using System.Linq.Expressions;

namespace NutritionService.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
      
        T Add(T entity);
        T Update(T entity);
        Task DeleteAsync(int id);
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        void SaveInclude(T entity, params string[] includedProperties);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
