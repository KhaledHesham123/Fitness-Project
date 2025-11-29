namespace UserProfileService.Contract
{
    public interface IunitofWork:IDisposable
    {
        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
