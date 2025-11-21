using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UserProfileService.Data.Context;
using UserProfileService.Shared.Entites;

namespace UserProfileService.Shared.GenericRepos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly UserProfileDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public IDbContextTransaction? _Transaction { get; set; }

        public GenericRepository(UserProfileDbContext _dbContext)
        {
            this._dbContext = _dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().Where(e=>e.IsDeleted == false).AsQueryable();
        }

        public IQueryable<T> GetByIdQueryable(Guid id)
        {
            return _dbContext.Set<T>().Where(e =>e.Id == id&&e.IsDeleted==false).AsQueryable();


        }
        public async Task addAsync(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
        }

        public async Task<T> GetByCriteriaAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).FirstAsync();
        }


        public  async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }



        public Task UpdateAsync(T item)
        {
            _dbContext.Set<T>().Update(item);
            return Task.CompletedTask;
        }

        public void SaveInclude(T entity)
        {
            var existingEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);
            if (existingEntity == null) 
            {
                existingEntity = _dbSet.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
                if (existingEntity == null)
                    throw new Exception($"Entity of type {typeof(T).Name} with Id {entity.Id} not found.");

                _dbSet.Attach(existingEntity);

            }

            var entry = _dbContext.Entry(existingEntity);
            var keyNames = entry.Metadata.FindPrimaryKey().Properties.Select(p => p.Name).ToList();


            foreach (var prop in typeof(T).GetProperties())
            {
                if (entry.Metadata.FindProperty(prop.Name) == null)
                    continue;

                if (keyNames.Contains(prop.Name))
                    continue;

                var oldvalue = prop.GetValue(existingEntity);
                var newvale = prop.GetValue(entity);

                if (newvale != null && !object.Equals(oldvalue, newvale))
                {
                    prop.SetValue(existingEntity, newvale);
                    entry.Property(prop.Name).IsModified = true;
                }
                else
                {
                    entry.Property(prop.Name).IsModified = false;
                }


            }


        }


        public Task DeleteAsync(T item)
        {
            _dbContext.Set<T>().Remove(item);
            return Task.CompletedTask;
        }



        public Task<int> SaveChanges()
        {
            return _dbContext.SaveChangesAsync();

        }

       
        
    }
}
