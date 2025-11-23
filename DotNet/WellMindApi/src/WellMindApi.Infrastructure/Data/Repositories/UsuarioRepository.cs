using Microsoft.EntityFrameworkCore;
using WellMindApi.Domain.Entities;
using WellMindApi.Domain.Interfaces.Repositories;
using WellMindApi.Infrastructure.Data.Context;

namespace WellMindApi.Infrastructure.Data.Repositories;

/// <summary>
/// Implementação do repositório de Usuários
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly WellMindDbContext _context;

    public UsuarioRepository(WellMindDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorIdAsync(int id)
    {
        return await _context.Usuarios
            .Include(u => u.RegistrosBemEstar.OrderByDescending(r => r.DataRegistro).Take(10))
            .Include(u => u.Alertas.Where(a => a.Status == "PENDENTE"))
            .FirstOrDefaultAsync(u => u.IdUsuario == id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email.ToLower());
    }

    public async Task<IEnumerable<Usuario>> ObterTodosAsync(int pageNumber, int pageSize)
    {
        return await _context.Usuarios
            .Where(u => u.Ativo)
            .OrderBy(u => u.Nome)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> ContarTotalAsync()
    {
        return await _context.Usuarios.CountAsync();
    }

    public async Task<int> ContarAtivosAsync()
    {
        return await _context.Usuarios.CountAsync(u => u.Ativo);
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public Task AtualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public Task RemoverAsync(Usuario usuario)
    {
        // Soft delete
        usuario.Desativar();
        _context.Usuarios.Update(usuario);
        return Task.CompletedTask;
    }

    public async Task<bool> EmailExisteAsync(string email)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email == email.ToLower());
    }

    public async Task<IEnumerable<Usuario>> ObterUsuariosEmRiscoBurnoutAsync()
    {
        // Usuários com 3+ registros críticos nos últimos 7 dias
        var dataLimite = DateTime.Now.AddDays(-7);

        return await _context.Usuarios
            .Where(u => u.Ativo)
            .Where(u => u.RegistrosBemEstar
                .Where(r => r.DataRegistro >= dataLimite)
                .Count(r => r.NivelEstresse >= 8 && r.NivelEnergia <= 3 && r.NivelHumor <= 4) >= 3)
            .Include(u => u.RegistrosBemEstar.OrderByDescending(r => r.DataRegistro).Take(7))
            .ToListAsync();
    }
}
