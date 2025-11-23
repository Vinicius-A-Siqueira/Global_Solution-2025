using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Infrastructure.Data.Configurations;

public class TransicaoCarreiraConfiguration : IEntityTypeConfiguration<TransicaoCarreira>
{
    public void Configure(EntityTypeBuilder<TransicaoCarreira> builder)
    {
        builder.ToTable("TRANSICAO_CARREIRA");

        builder.HasKey(t => t.IdTransicao);

        builder.Property(t => t.IdTransicao)
            .HasColumnName("ID_TRANSICAO")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.IdUsuario)
            .HasColumnName("ID_USUARIO")
            .IsRequired();

        builder.Property(t => t.DataTransicao)
            .HasColumnName("DATA_TRANSICAO")
            .IsRequired()
            .HasDefaultValueSql("SYSDATE");

        builder.Property(t => t.TipoTransicao)
            .HasColumnName("TIPO_TRANSICAO")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.CargoAnterior)
            .HasColumnName("CARGO_ANTERIOR")
            .HasMaxLength(100);

        builder.Property(t => t.CargoNovo)
            .HasColumnName("CARGO_NOVO")
            .HasMaxLength(100);

        builder.Property(t => t.Descricao)
            .HasColumnName("DESCRICAO")
            .HasMaxLength(500);

        builder.Property(t => t.StatusTransicao)
            .HasColumnName("STATUS_TRANSICAO")
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("EM_ANDAMENTO");

        // Relacionamento
        builder.HasOne(t => t.Usuario)
            .WithMany(u => u.TransicoesCarreira)
            .HasForeignKey(t => t.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice
        builder.HasIndex(t => new { t.IdUsuario, t.StatusTransicao })
            .HasDatabaseName("IX_TRANSICAO_USUARIO_STATUS");
    }
}
