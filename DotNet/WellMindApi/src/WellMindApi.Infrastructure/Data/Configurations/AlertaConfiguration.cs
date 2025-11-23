using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Infrastructure.Data.Configurations;

public class AlertaConfiguration : IEntityTypeConfiguration<Alerta>
{
    public void Configure(EntityTypeBuilder<Alerta> builder)
    {
        builder.ToTable("ALERTA");

        builder.HasKey(a => a.IdAlerta);

        builder.Property(a => a.IdAlerta)
            .HasColumnName("ID_ALERTA")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.IdUsuario)
            .HasColumnName("ID_USUARIO")
            .IsRequired();

        builder.Property(a => a.TipoAlerta)
            .HasColumnName("TIPO_ALERTA")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Descricao)
            .HasColumnName("DESCRICAO")
            .HasMaxLength(500);

        builder.Property(a => a.NivelGravidade)
            .HasColumnName("NIVEL_GRAVIDADE")
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Status)
            .HasColumnName("STATUS_ALERTA")
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("PENDENTE");

        builder.Property(a => a.DataAlerta)
            .HasColumnName("DATA_ALERTA")
            .IsRequired()
            .HasDefaultValueSql("SYSDATE");

        builder.Property(a => a.DataResolucao)
            .HasColumnName("DATA_RESOLUCAO");

        builder.Property(a => a.AcaoTomada)
            .HasColumnName("ACAO_TOMADA")
            .HasMaxLength(500);

        // Relacionamento
        builder.HasOne(a => a.Usuario)
            .WithMany(u => u.Alertas)
            .HasForeignKey(a => a.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(a => a.Status)
            .HasDatabaseName("IX_ALERTA_STATUS");

        builder.HasIndex(a => a.NivelGravidade)
            .HasDatabaseName("IX_ALERTA_GRAVIDADE");

        builder.HasIndex(a => new { a.IdUsuario, a.Status })
            .HasDatabaseName("IX_ALERTA_USUARIO_STATUS");
    }
}
