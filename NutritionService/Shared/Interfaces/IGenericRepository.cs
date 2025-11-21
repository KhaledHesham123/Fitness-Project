using NutritionService.Domain.Entities;
using System.Linq.Expressions;

namespace NutritionService.Shared.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        T Add(T entity);
        Task DeleteAsync(Guid id);
        IQueryable<T> Get(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        void SaveChanges();
        Task SaveChangesAsync();
        void SaveInclude(T entity, params string[] includedProperties);
        T Update(T entity);
    }
}