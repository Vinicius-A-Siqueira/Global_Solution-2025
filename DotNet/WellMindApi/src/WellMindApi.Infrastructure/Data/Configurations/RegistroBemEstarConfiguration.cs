using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Infrastructure.Data.Configurations;

public class RegistroBemEstarConfiguration : IEntityTypeConfiguration<RegistroBemEstar>
{
    public void Configure(EntityTypeBuilder<RegistroBemEstar> builder)
    {
        builder.ToTable("REGISTRO_BEMESTAR");

        builder.HasKey(r => r.IdRegistro);

        builder.Property(r => r.IdRegistro)
            .HasColumnName("ID_REGISTRO")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.IdUsuario)
            .HasColumnName("ID_USUARIO")
            .IsRequired();

        builder.Property(r => r.DataRegistro)
            .HasColumnName("DATA_REGISTRO")
            .IsRequired()
            .HasDefaultValueSql("SYSDATE");

        builder.Property(r => r.NivelHumor)
            .HasColumnName("NIVEL_HUMOR")
            .IsRequired();

        builder.Property(r => r.NivelEstresse)
            .HasColumnName("NIVEL_ESTRESSE")
            .IsRequired();

        builder.Property(r => r.NivelEnergia)
            .HasColumnName("NIVEL_ENERGIA")
            .IsRequired();

        builder.Property(r => r.HorasSono)
            .HasColumnName("HORAS_SONO")
            .HasColumnType("NUMBER(4,2)")
            .IsRequired();

        builder.Property(r => r.QualidadeSono)
            .HasColumnName("QUALIDADE_SONO")
            .IsRequired();

        builder.Property(r => r.Observacoes)
            .HasColumnName("OBSERVACOES")
            .HasMaxLength(500);

        // Relacionamento
        builder.HasOne(r => r.Usuario)
            .WithMany(u => u.RegistrosBemEstar)
            .HasForeignKey(r => r.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice para consultas por usuário e data
        builder.HasIndex(r => new { r.IdUsuario, r.DataRegistro })
            .HasDatabaseName("IX_REGISTRO_USUARIO_DATA");
    }
}
