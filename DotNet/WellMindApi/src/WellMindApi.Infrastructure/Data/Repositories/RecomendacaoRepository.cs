using Microsoft.EntityFrameworkCore;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;
using WellMindApi.Infrastructure.Data.Context;

namespace WellMindApi.Infrastructure.Data.Repositories;

public class RecomendacaoRepository : IRecomendacaoRepository
{
    private readonly WellMindDbContext _context;

    public RecomendacaoRepository(WellMindDbContext context)
    {
        _context = context;
    }

    public async Task<Recomendacao?> ObterPorIdAsync(int id)
    {
        return await _context.Recomendacoes.FindAsync(id);
    }

    public async Task<IEnumerable<Recomendacao>> ObterPorUsuarioAsync(int idUsuario)
    {
        return await _context.Recomendacoes
            .Where(r => r.IdUsuario == idUsuario)
            .OrderByDescending(r => r.DataRecomendacao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Recomendacao>> ObterNaoLidasPorUsuarioAsync(int idUsuario)
    {
        return await _context.Recomendacoes
            .Where(r => r.IdUsuario == idUsuario && !r.Lida)
            .OrderByDescending(r => r.DataRecomendacao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Recomendacao>> ObterPorTipoAsync(string tipoRecomendacao)
    {
        return await _context.Recomendacoes
            .Where(r => r.TipoRecomendacao == tipoRecomendacao)
            .OrderByDescending(r => r.DataRecomendacao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Recomendacao>> ObterRecentesAsync(int idUsuario, int ultimosDias = 7)
    {
        var dataLimite = DateTime.Now.AddDays(-ultimosDias);

        return await _context.Recomendacoes
            .Where(r => r.IdUsuario == idUsuario && r.DataRecomendacao >= dataLimite)
            .OrderByDescending(r => r.DataRecomendacao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Recomendacao>> ObterTodosAsync(int pageNumber, int pageSize)
    {
        return await _context.Recomendacoes
            .OrderByDescending(r => r.DataRecomendacao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AdicionarAsync(Recomendacao recomendacao)
    {
        await _context.Recomendacoes.AddAsync(recomendacao);
    }

    public Task AtualizarAsync(Recomendacao recomendacao)
    {
        _context.Recomendacoes.Update(recomendacao);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Recomendacao recomendacao)
    {
        _context.Recomendacoes.Remove(recomendacao);
        return Task.CompletedTask;
    }

    public async Task MarcarVariasComoLidasAsync(IEnumerable<int> idsRecomendacoes)
    {
        await _context.Recomendacoes
            .Where(r => idsRecomendacoes.Contains(r.IdRecomendacao))
            .ExecuteUpdateAsync(setters => setters.SetProperty(r => r.Lida, true));
    }

    public async Task<int> ContarNaoLidasAsync(int idUsuario)
    {
        return await _context.Recomendacoes
            .CountAsync(r => r.IdUsuario == idUsuario && !r.Lida);
    }

    public async Task<Dictionary<string, int>> ObterEstatisticasPorTipoAsync(int idUsuario)
    {
        return await _context.Recomendacoes
            .Where(r => r.IdUsuario == idUsuario)
            .GroupBy(r => r.TipoRecomendacao)
            .Select(g => new { Tipo = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Tipo, x => x.Count);
    }
}
