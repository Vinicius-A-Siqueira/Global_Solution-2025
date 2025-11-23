using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Infrastructure.Data.Configurations;

public class SessaoConfiguration : IEntityTypeConfiguration<Sessao>
{
    public void Configure(EntityTypeBuilder<Sessao> builder)
    {
        builder.ToTable("SESSAO");

        builder.HasKey(s => s.IdSessao);

        builder.Property(s => s.IdSessao)
            .HasColumnName("ID_SESSAO")
            .ValueGeneratedOnAdd();

        builder.Property(s => s.IdUsuario)
            .HasColumnName("ID_USUARIO")
            .IsRequired();

        builder.Property(s => s.DataSessao)
            .HasColumnName("DATA_SESSAO")
            .IsRequired();

        builder.Property(s => s.TipoSessao)
            .HasColumnName("TIPO_SESSAO")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Tema)
            .HasColumnName("TEMA")
            .HasMaxLength(200);

        builder.Property(s => s.Observacoes)
            .HasColumnName("OBSERVACOES")
            .HasMaxLength(1000);

        builder.Property(s => s.ProximosPassos)
            .HasColumnName("PROXIMOS_PASSOS")
            .HasMaxLength(500);

        // Relacionamento
        builder.HasOne(s => s.Usuario)
            .WithMany(u => u.Sessoes)
            .HasForeignKey(s => s.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice
        builder.HasIndex(s => new { s.IdUsuario, s.DataSessao })
            .HasDatabaseName("IX_SESSAO_USUARIO_DATA");
    }
}
