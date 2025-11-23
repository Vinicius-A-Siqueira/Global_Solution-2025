using Microsoft.EntityFrameworkCore.Storage;
using WellMindApi.Domain.Interfaces;
using WellMindApi.Infrastructure.Data.Context;

namespace WellMindApi.Infrastructure.Data;

/// <summary>
/// Implementação do padrão Unit of Work usando EF Core
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly WellMindDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(WellMindDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task<int> CommitAsync()
    {
        try
        {
            var result = await _context.SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            return result;
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
