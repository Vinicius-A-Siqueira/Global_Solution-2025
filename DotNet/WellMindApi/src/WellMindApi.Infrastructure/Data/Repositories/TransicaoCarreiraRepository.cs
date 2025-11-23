using Microsoft.EntityFrameworkCore;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;
using WellMindApi.Infrastructure.Data.Context;

namespace WellMindApi.Infrastructure.Data.Repositories;

public class TransicaoCarreiraRepository : ITransicaoCarreiraRepository
{
    private readonly WellMindDbContext _context;

    public TransicaoCarreiraRepository(WellMindDbContext context)
    {
        _context = context;
    }

    public async Task<TransicaoCarreira?> ObterPorIdAsync(int id)
    {
        return await _context.TransicoesCarreira
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.IdTransicao == id);
    }

    public async Task<IEnumerable<TransicaoCarreira>> ObterPorUsuarioAsync(int idUsuario)
    {
        return await _context.TransicoesCarreira
            .Where(t => t.IdUsuario == idUsuario)
            .OrderByDescending(t => t.DataTransicao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TransicaoCarreira>> ObterEmAndamentoAsync(int idUsuario)
    {
        return await _context.TransicoesCarreira
            .Where(t => t.IdUsuario == idUsuario && t.StatusTransicao == "EM_ANDAMENTO")
            .OrderByDescending(t => t.DataTransicao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TransicaoCarreira>> ObterConcluidasAsync(int idUsuario)
    {
        return await _context.TransicoesCarreira
            .Where(t => t.IdUsuario == idUsuario && t.StatusTransicao == "CONCLUIDA")
            .OrderByDescending(t => t.DataTransicao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TransicaoCarreira>> ObterPorTipoAsync(int idUsuario, string tipoTransicao)
    {
        return await _context.TransicoesCarreira
            .Where(t => t.IdUsuario == idUsuario && t.TipoTransicao == tipoTransicao)
            .OrderByDescending(t => t.DataTransicao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<TransicaoCarreira>> ObterPorPeriodoAsync(
        int idUsuario,
        DateTime dataInicio,
        DateTime dataFim)
    {
        return await _context.TransicoesCarreira
            .Where(t => t.IdUsuario == idUsuario &&
                       t.DataTransicao >= dataInicio &&
                       t.DataTransicao <= dataFim)
            .OrderBy(t => t.DataTransicao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TransicaoCarreira?> ObterTransicaoAtivaAsync(int idUsuario)
    {
        return await _context.TransicoesCarreira
            .Where(t => t.IdUsuario == idUsuario && t.StatusTransicao == "EM_ANDAMENTO")
            .OrderByDescending(t => t.DataTransicao)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TransicaoCarreira>> ObterTodosAsync(int pageNumber, int pageSize)
    {
        return await _context.TransicoesCarreira
            .OrderByDescending(t => t.DataTransicao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AdicionarAsync(TransicaoCarreira transicao)
    {
        await _context.TransicoesCarreira.AddAsync(transicao);
    }

    public Task AtualizarAsync(TransicaoCarreira transicao)
    {
        _context.TransicoesCarreira.Update(transicao);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(TransicaoCarreira transicao)
    {
        _context.TransicoesCarreira.Remove(transicao);
        return Task.CompletedTask;
    }

    public async Task<int> ContarTransicoesPorUsuarioAsync(int idUsuario)
    {
        return await _context.TransicoesCarreira.CountAsync(t => t.IdUsuario == idUsuario);
    }

    public async Task<Dictionary<string, int>> ObterEstatisticasPorTipoAsync(int idUsuario)
    {
        return await _context.TransicoesCarreira
            .Where(t => t.IdUsuario == idUsuario)
            .GroupBy(t => t.TipoTransicao)
            .Select(g => new { Tipo = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Tipo, x => x.Count);
    }

    public async Task<bool> TemTransicaoEmAndamentoAsync(int idUsuario)
    {
        return await _context.TransicoesCarreira
            .AnyAsync(t => t.IdUsuario == idUsuario && t.StatusTransicao == "EM_ANDAMENTO");
    }

    public async Task<IEnumerable<TransicaoCarreira>> ObterHistoricoCarreiraAsync(int idUsuario)
    {
        return await _context.TransicoesCarreira
            .Where(t => t.IdUsuario == idUsuario)
            .OrderBy(t => t.DataTransicao)
            .AsNoTracking()
            .ToListAsync();
    }
}
