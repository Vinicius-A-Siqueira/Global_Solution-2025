using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellMindApi.Domain.Entities;

namespace WellMindApi.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração EF Core para a entidade Usuario
/// </summary>
public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // Tabela
        builder.ToTable("USUARIO");

        // Chave Primária
        builder.HasKey(u => u.IdUsuario);

        builder.Property(u => u.IdUsuario)
            .HasColumnName("ID_USUARIO")
            .ValueGeneratedOnAdd();

        // Propriedades
        builder.Property(u => u.Nome)
            .HasColumnName("NOME")
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Nome completo do usuário");

        builder.Property(u => u.Email)
            .HasColumnName("EMAIL")
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Email único do usuário");

        // Índice único no email
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_USUARIO_EMAIL");

        builder.Property(u => u.SenhaHash)
            .HasColumnName("SENHA_HASH")
            .IsRequired()
            .HasMaxLength(255)
            .HasComment("Hash BCrypt da senha");

        builder.Property(u => u.DataNascimento)
            .HasColumnName("DATA_NASCIMENTO")
            .IsRequired()
            .HasColumnType("DATE")
            .HasComment("Data de nascimento do usuário");

        builder.Property(u => u.Genero)
            .HasColumnName("GENERO")
            .HasMaxLength(20);

        builder.Property(u => u.Telefone)
            .HasColumnName("TELEFONE")
            .HasMaxLength(20);

        builder.Property(u => u.DataCadastro)
            .HasColumnName("DATA_CADASTRO")
            .IsRequired()
            .HasDefaultValueSql("SYSDATE")
            .HasComment("Data de cadastro do usuário");

        // Conversão do bool para CHAR(1) - 'S' ou 'N'
        builder.Property(u => u.Ativo)
            .HasColumnName("STATUS_ATIVO")
            .HasConversion(
                v => v ? 'S' : 'N',
                v => v == 'S'
            )
            .HasMaxLength(1)
            .HasDefaultValue(true); // << CORRETO: BOOL => TRUE

        // Relacionamentos
        builder.HasMany(u => u.RegistrosBemEstar)
            .WithOne(r => r.Usuario)
            .HasForeignKey(r => r.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_REGISTRO_USUARIO");

        builder.HasMany(u => u.Alertas)
            .WithOne(a => a.Usuario)
            .HasForeignKey(a => a.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ALERTA_USUARIO");

        builder.HasMany(u => u.Sessoes)
            .WithOne(s => s.Usuario)
            .HasForeignKey(s => s.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_SESSAO_USUARIO");

        builder.HasMany(u => u.TransicoesCarreira)
            .WithOne(t => t.Usuario)
            .HasForeignKey(t => t.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_TRANSICAO_USUARIO");
    }
}
