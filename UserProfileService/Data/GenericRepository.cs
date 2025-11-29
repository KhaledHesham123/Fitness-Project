using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using UserProfileService.Contract;
using UserProfileService.Entity;
namespace UserProfileService.Data
{
        public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
        {
            private readonly UserProfileDbContext _context;
            private readonly DbSet<T> _dbSet;

            public GenericRepository(UserProfileDbContext Context)
            {
                _context = Context;
                _dbSet = _context.Set<T>();

            }
            public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
        public void  Add(T entity)
            {
                 _dbSet.Add(entity);
            }

            //soft delete
            public virtual void Delete(T entity)
            {
                _context.Attach(entity);
                _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
                _context.Entry(entity).Property("DeletedAt").CurrentValue = DateTime.Now;
            }
            public virtual void HardDelete(T entity)
            {
                var isDeleted = (bool)_context.Entry(entity).Property("IsDeleted").CurrentValue;
                if (isDeleted)
                    _dbSet.Remove(entity);
                else
                    Delete(entity);
            }
            public async Task<T> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> criteria, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
            {
                IQueryable<T> query = _dbSet;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return await query.FirstOrDefaultAsync(criteria);
            }

            public IQueryable<T> GetAll()
            {
                return _dbSet;
            }

            public async Task<T> GetByIdAsync(Guid id, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
            {
                IQueryable<T> query = _dbSet;
                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }

                }
                return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
            }



            public virtual void Update(T entity)
            {
                _dbSet.Update(entity);
            }



            public void SaveInclude(T entity, params string[] includedProperties)
            {
                var LocalEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);
                EntityEntry entry;

                if (LocalEntity == null)
                {
                    entry = _context.Entry(entity);
                }
                else
                {
                    entry = _context.ChangeTracker.Entries<T>().First(e => e.Entity.Id == entity.Id);
                }

                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.IsPrimaryKey())
                        continue;
                    else
                    {
                        if (includedProperties.Contains(property.Metadata.Name))
                        {
                            property.IsModified = true;
                        }
                        else
                        {
                            property.IsModified = false;
                        }
                    }
                }
            }


            public Task SaveChangesAsync()
            {
               return _context.SaveChangesAsync();
            }
    }
}
