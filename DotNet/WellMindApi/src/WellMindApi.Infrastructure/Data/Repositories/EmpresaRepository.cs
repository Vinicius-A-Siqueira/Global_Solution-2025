using Microsoft.EntityFrameworkCore;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;
using WellMindApi.Infrastructure.Data.Context;

namespace WellMindApi.Infrastructure.Data.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly WellMindDbContext _context;

    public EmpresaRepository(WellMindDbContext context)
    {
        _context = context;
    }

    public async Task<Empresa?> ObterPorIdAsync(int id)
    {
        return await _context.Empresas.FindAsync(id);
    }

    public async Task<Empresa?> ObterPorCNPJAsync(string cnpj)
    {
        var cnpjLimpo = new string(cnpj.Where(char.IsDigit).ToArray());
        return await _context.Empresas
            .FirstOrDefaultAsync(e => e.CNPJ == cnpjLimpo);
    }

    public async Task<IEnumerable<Empresa>> ObterTodosAsync(int pageNumber, int pageSize)
    {
        return await _context.Empresas
            .OrderBy(e => e.NomeEmpresa)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> ContarTotalAsync()
    {
        return await _context.Empresas.CountAsync();
    }

    public async Task AdicionarAsync(Empresa empresa)
    {
        await _context.Empresas.AddAsync(empresa);
    }

    public Task AtualizarAsync(Empresa empresa)
    {
        _context.Empresas.Update(empresa);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Empresa empresa)
    {
        _context.Empresas.Remove(empresa);
        return Task.CompletedTask;
    }

    public async Task<bool> CNPJExisteAsync(string cnpj)
    {
        var cnpjLimpo = new string(cnpj.Where(char.IsDigit).ToArray());
        return await _context.Empresas.AnyAsync(e => e.CNPJ == cnpjLimpo);
    }

    public async Task<IEnumerable<Empresa>> BuscarPorNomeAsync(string nome)
    {
        return await _context.Empresas
            .Where(e => EF.Functions.Like(e.NomeEmpresa, $"%{nome}%"))
            .OrderBy(e => e.NomeEmpresa)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<EmpresaStatistics> ObterEstatisticasAsync(int idEmpresa)
    {
        // Implementação simplificada - em produção, você teria relacionamento Empresa-Usuario
        return new EmpresaStatistics
        {
            TotalColaboradores = 0,
            ColaboradoresAtivos = 0,
            AlertasPendentes = 0,
            AlertasCriticos = 0,
            IndiceBemEstarMedio = 0
        };
    }
}
