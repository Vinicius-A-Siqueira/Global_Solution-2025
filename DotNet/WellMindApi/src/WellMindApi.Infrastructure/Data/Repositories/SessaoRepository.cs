using Microsoft.EntityFrameworkCore;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;
using WellMindApi.Infrastructure.Data.Context;

namespace WellMindApi.Infrastructure.Data.Repositories;

public class SessaoRepository : ISessaoRepository
{
    private readonly WellMindDbContext _context;

    public SessaoRepository(WellMindDbContext context)
    {
        _context = context;
    }

    public async Task<Sessao?> ObterPorIdAsync(int id)
    {
        return await _context.Sessoes
            .Include(s => s.Usuario)
            .FirstOrDefaultAsync(s => s.IdSessao == id);
    }

    public async Task<IEnumerable<Sessao>> ObterPorUsuarioAsync(int idUsuario)
    {
        return await _context.Sessoes
            .Where(s => s.IdUsuario == idUsuario)
            .OrderByDescending(s => s.DataSessao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Sessao>> ObterFuturasAsync(int idUsuario)
    {
        return await _context.Sessoes
            .Where(s => s.IdUsuario == idUsuario && s.DataSessao > DateTime.Now)
            .OrderBy(s => s.DataSessao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Sessao>> ObterPassadasAsync(int idUsuario)
    {
        return await _context.Sessoes
            .Where(s => s.IdUsuario == idUsuario && s.DataSessao <= DateTime.Now)
            .OrderByDescending(s => s.DataSessao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Sessao>> ObterPorTipoAsync(int idUsuario, string tipoSessao)
    {
        return await _context.Sessoes
            .Where(s => s.IdUsuario == idUsuario && s.TipoSessao == tipoSessao)
            .OrderByDescending(s => s.DataSessao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Sessao>> ObterPorPeriodoAsync(
        int idUsuario,
        DateTime dataInicio,
        DateTime dataFim)
    {
        return await _context.Sessoes
            .Where(s => s.IdUsuario == idUsuario &&
                       s.DataSessao >= dataInicio &&
                       s.DataSessao <= dataFim)
            .OrderBy(s => s.DataSessao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Sessao?> ObterProximaSessaoAsync(int idUsuario)
    {
        return await _context.Sessoes
            .Where(s => s.IdUsuario == idUsuario && s.DataSessao > DateTime.Now)
            .OrderBy(s => s.DataSessao)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Sessao>> ObterTodosAsync(int pageNumber, int pageSize)
    {
        return await _context.Sessoes
            .OrderByDescending(s => s.DataSessao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AdicionarAsync(Sessao sessao)
    {
        await _context.Sessoes.AddAsync(sessao);
    }

    public Task AtualizarAsync(Sessao sessao)
    {
        _context.Sessoes.Update(sessao);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Sessao sessao)
    {
        _context.Sessoes.Remove(sessao);
        return Task.CompletedTask;
    }

    public async Task<int> ContarSessoesPorUsuarioAsync(int idUsuario)
    {
        return await _context.Sessoes.CountAsync(s => s.IdUsuario == idUsuario);
    }

    public async Task<Dictionary<string, int>> ObterEstatisticasPorTipoAsync(int idUsuario)
    {
        return await _context.Sessoes
            .Where(s => s.IdUsuario == idUsuario)
            .GroupBy(s => s.TipoSessao)
            .Select(g => new { Tipo = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Tipo, x => x.Count);
    }

    public async Task<bool> TemSessaoAgendadaAsync(int idUsuario, DateTime dataSessao)
    {
        // Verifica se há sessão no mesmo dia
        var dataInicio = dataSessao.Date;
        var dataFim = dataSessao.Date.AddDays(1);

        return await _context.Sessoes
            .AnyAsync(s => s.IdUsuario == idUsuario &&
                          s.DataSessao >= dataInicio &&
                          s.DataSessao < dataFim);
    }
}
