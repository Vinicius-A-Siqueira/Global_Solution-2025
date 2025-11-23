using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Infrastructure.Data.Configurations;

public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
{
    public void Configure(EntityTypeBuilder<Empresa> builder)
    {
        builder.ToTable("EMPRESA");

        builder.HasKey(e => e.IdEmpresa);

        builder.Property(e => e.IdEmpresa)
            .HasColumnName("ID_EMPRESA")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.NomeEmpresa)
            .HasColumnName("NOME_EMPRESA")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.CNPJ)
            .HasColumnName("CNPJ")
            .IsRequired()
            .HasMaxLength(14);

        // Índice único no CNPJ
        builder.HasIndex(e => e.CNPJ)
            .IsUnique()
            .HasDatabaseName("IX_EMPRESA_CNPJ");

        builder.Property(e => e.Endereco)
            .HasColumnName("ENDERECO")
            .HasMaxLength(300);

        builder.Property(e => e.Telefone)
            .HasColumnName("TELEFONE")
            .HasMaxLength(20);

        builder.Property(e => e.EmailContato)
            .HasColumnName("EMAIL_CONTATO")
            .HasMaxLength(100);

        builder.Property(e => e.DataCadastro)
            .HasColumnName("DATA_CADASTRO")
            .IsRequired()
            .HasDefaultValueSql("SYSDATE");
    }
}
