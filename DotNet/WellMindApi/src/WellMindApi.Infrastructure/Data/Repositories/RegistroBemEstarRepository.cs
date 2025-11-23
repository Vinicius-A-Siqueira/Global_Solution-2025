using Microsoft.EntityFrameworkCore;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;
using WellMindApi.Infrastructure.Data.Context;

namespace WellMindApi.Infrastructure.Data.Repositories;

public class RegistroBemEstarRepository : IRegistroBemEstarRepository
{
    private readonly WellMindDbContext _context;

    public RegistroBemEstarRepository(WellMindDbContext context)
    {
        _context = context;
    }

    public async Task<RegistroBemEstar?> ObterPorIdAsync(int id)
    {
        return await _context.RegistrosBemEstar
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.IdRegistro == id);
    }

    public async Task<IEnumerable<RegistroBemEstar>> ObterPorUsuarioAsync(int idUsuario, int ultimosDias = 7)
    {
        var dataLimite = DateTime.Now.AddDays(-ultimosDias);

        return await _context.RegistrosBemEstar
            .Where(r => r.IdUsuario == idUsuario && r.DataRegistro >= dataLimite)
            .OrderByDescending(r => r.DataRegistro)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<RegistroBemEstar>> ObterTodosAsync(int pageNumber, int pageSize)
    {
        return await _context.RegistrosBemEstar
            .OrderByDescending(r => r.DataRegistro)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<RegistroBemEstar?> ObterUltimoRegistroUsuarioAsync(int idUsuario)
    {
        return await _context.RegistrosBemEstar
            .Where(r => r.IdUsuario == idUsuario)
            .OrderByDescending(r => r.DataRegistro)
            .FirstOrDefaultAsync();
    }

    public async Task AdicionarAsync(RegistroBemEstar registro)
    {
        await _context.RegistrosBemEstar.AddAsync(registro);
    }

    public Task AtualizarAsync(RegistroBemEstar registro)
    {
        _context.RegistrosBemEstar.Update(registro);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(RegistroBemEstar registro)
    {
        _context.RegistrosBemEstar.Remove(registro);
        return Task.CompletedTask;
    }

    public async Task<decimal> CalcularMediaIndiceBemEstarAsync(int idUsuario, int ultimosDias = 30)
    {
        var dataLimite = DateTime.Now.AddDays(-ultimosDias);

        var registros = await _context.RegistrosBemEstar
            .Where(r => r.IdUsuario == idUsuario && r.DataRegistro >= dataLimite)
            .ToListAsync();

        if (!registros.Any())
            return 0;

        return registros.Average(r => r.CalcularIndiceBemEstar());
    }
}
