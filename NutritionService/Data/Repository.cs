using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NutritionService.Data.Context;
using NutritionService.Models;
using System.Linq.Expressions;

namespace NutritionService.Data
{
    public class Repository<T> where T : BaseEntity, new()
    {
        protected readonly NutritionDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(NutritionDbContext context)

        {
            _context = context;
            _dbSet = context.Set<T>();


        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _dbSet.Update(entity);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.ID == id && !e.IsDeleted);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.Where(e => !e.IsDeleted);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public void SaveInclude(T entity, params string[] includedProperties)
        {
            var localEntity = _dbSet.Local.FirstOrDefault(e => e.ID == entity.ID);
            EntityEntry entry;

            if (localEntity is null)
            {
                entry = _context.Entry(entity);
            }
            else
            {
                entry = _context.ChangeTracker.Entries<T>().First(e => e.Entity.ID == entity.ID);
            }


            foreach (var property in entry.Properties)
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

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}

