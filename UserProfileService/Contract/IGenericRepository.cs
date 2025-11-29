using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserProfileService.Entity;
namespace UserProfileService.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public void Add(T entity);

        //soft delete
        public void Delete(T entity);
        public void HardDelete(T entity);
        public Task<T> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> criteria, params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        public IQueryable<T> GetAll();

        public Task<T> GetByIdAsync(Guid id, params System.Linq.Expressions.Expression<Func<T, object>>[] includes);



        public void Update(T entity);

        public Task SaveChangesAsync();


        public void SaveInclude(T entity, params string[] includedProperties);
    }
}
