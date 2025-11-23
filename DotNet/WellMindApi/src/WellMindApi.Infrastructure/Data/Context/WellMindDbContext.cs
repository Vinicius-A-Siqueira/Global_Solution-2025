using Microsoft.EntityFrameworkCore;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Infrastructure.Data.Context;

/// <summary>
/// Contexto do Entity Framework Core para o banco Oracle
/// </summary>
public class WellMindDbContext : DbContext
{
    public WellMindDbContext(DbContextOptions<WellMindDbContext> options)
        : base(options)
    {
    }

    // DbSets (Tabelas)
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<RegistroBemEstar> RegistrosBemEstar { get; set; }
    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<Recomendacao> Recomendacoes { get; set; }
    public DbSet<Sessao> Sessoes { get; set; }
    public DbSet<TransicaoCarreira> TransicoesCarreira { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WellMindDbContext).Assembly);

        // Configurações globais para Oracle
        ConfigurarConvencoesOracle(modelBuilder);
    }

    private static void ConfigurarConvencoesOracle(ModelBuilder modelBuilder)
    {
        // Oracle tem limite de 30 caracteres para nomes de objetos (versões antigas)
        // Oracle 12.2+ suporta até 128 caracteres

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Configurar nomes de tabelas em maiúsculo (padrão Oracle)
            if (entity.GetTableName() != null)
            {
                entity.SetTableName(entity.GetTableName()!.ToUpper());
            }

            // Configurar nomes de colunas em maiúsculo
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToUpper());
            }

            // Configurar índices
            foreach (var index in entity.GetIndexes())
            {
                if (index.GetDatabaseName() != null)
                {
                    index.SetDatabaseName(index.GetDatabaseName()!.ToUpper());
                }
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Aqui você pode adicionar lógica de auditoria, soft delete, etc.
        return base.SaveChangesAsync(cancellationToken);
    }
}
