using System.Linq.Expressions;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Shared.GenericRepos
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetByIdQueryable(Guid id);

        Task<T> GetByCriteriaAsync(Expression<Func<T,bool>> expression);

         Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);


        void SaveInclude(T entity);

        Task addAsync(T item);

        Task AddRangeAsync(IEnumerable<T> entities); 


        Task UpdateAsync(T item);

        Task DeleteAsync(T item);

         Task<int> SaveChanges();

        

       
    }
}
