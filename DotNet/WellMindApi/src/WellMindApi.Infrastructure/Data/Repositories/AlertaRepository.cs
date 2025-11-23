using Microsoft.EntityFrameworkCore;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;
using WellMindApi.Infrastructure.Data.Context;

namespace WellMindApi.Infrastructure.Data.Repositories;

public class AlertaRepository : IAlertaRepository
{
    private readonly WellMindDbContext _context;

    public AlertaRepository(WellMindDbContext context)
    {
        _context = context;
    }

    public async Task<Alerta?> ObterPorIdAsync(int id)
    {
        return await _context.Alertas
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.IdAlerta == id);
    }

    public async Task<IEnumerable<Alerta>> ObterPorUsuarioAsync(int idUsuario)
    {
        return await _context.Alertas
            .Where(a => a.IdUsuario == idUsuario)
            .OrderByDescending(a => a.DataAlerta)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Alerta>> ObterPendentesAsync()
    {
        return await _context.Alertas
            .Include(a => a.Usuario)
            .Where(a => a.Status == "PENDENTE")
            .OrderByDescending(a => a.DataAlerta)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Alerta>> ObterCriticosAsync()
    {
        return await _context.Alertas
            .Include(a => a.Usuario)
            .Where(a => a.NivelGravidade == "CRITICO" && a.Status == "PENDENTE")
            .OrderByDescending(a => a.DataAlerta)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Alerta>> ObterPorStatusAsync(string status)
    {
        return await _context.Alertas
            .Include(a => a.Usuario)
            .Where(a => a.Status == status.ToUpper())
            .OrderByDescending(a => a.DataAlerta)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AdicionarAsync(Alerta alerta)
    {
        await _context.Alertas.AddAsync(alerta);
    }

    public Task AtualizarAsync(Alerta alerta)
    {
        _context.Alertas.Update(alerta);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Alerta alerta)
    {
        _context.Alertas.Remove(alerta);
        return Task.CompletedTask;
    }

    public async Task<int> ContarAlertasCriticosPendentesAsync()
    {
        return await _context.Alertas
            .CountAsync(a => a.NivelGravidade == "CRITICO" && a.Status == "PENDENTE");
    }
}
