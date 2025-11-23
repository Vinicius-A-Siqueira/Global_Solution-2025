using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Infrastructure.Data.Configurations;

public class RecomendacaoConfiguration : IEntityTypeConfiguration<Recomendacao>
{
    public void Configure(EntityTypeBuilder<Recomendacao> builder)
    {
        builder.ToTable("RECOMENDACAO");

        builder.HasKey(r => r.IdRecomendacao);

        builder.Property(r => r.IdRecomendacao)
            .HasColumnName("ID_RECOMENDACAO")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.IdUsuario)
            .HasColumnName("ID_USUARIO")
            .IsRequired();

        builder.Property(r => r.DataRecomendacao)
            .HasColumnName("DATA_RECOMENDACAO")
            .IsRequired()
            .HasDefaultValueSql("SYSDATE");

        builder.Property(r => r.TipoRecomendacao)
            .HasColumnName("TIPO_RECOMENDACAO")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Conteudo)
            .HasColumnName("CONTEUDO")
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.Link)
            .HasColumnName("LINK")
            .HasMaxLength(500);

        builder.Property(r => r.Lida)
            .HasColumnName("LIDA")
            .HasConversion(
                v => v ? 'S' : 'N',
                v => v == 'S')
            .HasMaxLength(1)
            .HasDefaultValue(false); // <<<<<<<< CORREÇÃO IMPORTANTE

        // Relacionamento
        builder.HasOne(r => r.Usuario)
            .WithMany()
            .HasForeignKey(r => r.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice
        builder.HasIndex(r => new { r.IdUsuario, r.Lida })
            .HasDatabaseName("IX_RECOMENDACAO_USUARIO_LIDA");
    }
}
