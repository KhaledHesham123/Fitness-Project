using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WorkoutCatalogService.Data.Context;

namespace WorkoutCatalogService.Shared.UnitofWorks
{
    public class UnitofWork:IunitofWork
    {

        private readonly WorkoutCatalogDbContext _dbContext;
        public IDbContextTransaction? _Transaction { get; set; }

        public UnitofWork(WorkoutCatalogDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task BeginTransactionAsync()
        {
            _Transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_Transaction != null)
            {
                await _Transaction.CommitAsync();
                await _Transaction.DisposeAsync();
                _Transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_Transaction != null)
            {
                await _Transaction.RollbackAsync();
                await _Transaction.DisposeAsync();
                _Transaction = null;
            }

        }



        public void Dispose()
        {
            _Transaction?.Dispose();
            _dbContext.Dispose();
        }

    }
}
