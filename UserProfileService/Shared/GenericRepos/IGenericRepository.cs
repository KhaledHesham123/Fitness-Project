using UserProfileService.Shared.Entites;

namespace UserProfileService.Shared.GenericRepos
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetByIdQueryable(Guid id);

        void SaveInclude(T entity);

        Task addAsync(T item);

        Task UpdateAsync(T item);

        Task DeleteAsync(T item);

         Task<int> SaveChanges();

        

       
    }
}
