namespace WellMindApi.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync();
    Task RollbackAsync();
    Task BeginTransactionAsync();
}
