namespace UserProfileService.Shared.UnitofWorks
{
    public interface IunitofWork:IDisposable
    {
        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
